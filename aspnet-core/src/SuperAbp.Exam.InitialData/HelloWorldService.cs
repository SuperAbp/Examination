using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;

namespace SuperAbp.Exam.InitialData;

public class HelloWorldService : ITransientDependency
{
    public ILogger<HelloWorldService> Logger { get; set; }
    public IConfiguration? Configuration { get; set; }
    public HelloWorldService()
    {
        Logger = NullLogger<HelloWorldService>.Instance;
    }

    public async Task SayHelloAsync()
    {
        if (Configuration is null)
        {
            Logger.LogWarning("Configuration is null, cannot get connection string.");
            return;
        }

        using IDbConnection connection = new SqlConnection(Configuration.GetConnectionString("Default"));

        // get demo tenant id
        Guid? tenantId = await connection.QuerySingleOrDefaultAsync<Guid?>("SELECT Id FROM AbpTenants WHERE Name = @Name", new { Name = "Demo" });
        if (tenantId == null || tenantId == Guid.Empty)
        {
            Logger.LogWarning("Demo tenant not found, skipping initial data.");
            return;
        }

        await ClearDataAsync(connection, tenantId.Value);

        // Read questions.json and insert into database
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "questions.json");
        if (!File.Exists(jsonPath))
        {
            return;
        }
        string json = await File.ReadAllTextAsync(jsonPath);
        var doc = JsonSerializer.Deserialize<QuestionsFile>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (doc?.QuesiontBanks == null || doc.QuesiontBanks.Count == 0)
        {
            return;
        }

        var bankParams = new List<object>();
        var questionParams = new List<object>();
        var answerParams = new List<object>();
        var knowledgepointParams = new List<object>(); // question-knowledgepoint relations

        var seenKnowledgePoints = new HashSet<string>();

        // insert banks, questions and options (collect params first)
        foreach (var bank in doc.QuesiontBanks)
        {
            var bankId = Guid.NewGuid();
            bankParams.Add(new { Id = bankId, Name = bank.Name, Description = bank.Description, CreationTime = DateTime.Now, TenantId = tenantId.Value });

            if (bank.Questions == null) continue;

            foreach (var q in bank.Questions)
            {
                var qid = Guid.NewGuid();
                questionParams.Add(new { Id = qid, QuestionBankId = bankId, Title = q.Title, QuestionType = q.Type, Score = 2, CreationTime = DateTime.Now, TenantId = tenantId.Value });

                if (q.KnowledgePoints != null)
                {
                    var newKps = q.KnowledgePoints.Except(seenKnowledgePoints).ToList();
                    foreach (var kpId in newKps)
                    {
                        seenKnowledgePoints.Add(kpId);
                    }

                    // link all KP for this question
                    foreach (var kpId in q.KnowledgePoints)
                    {
                        knowledgepointParams.Add(new { QuestionId = qid, KnowledgePointId = kpId, CreationTime = DateTime.Now, TenantId = tenantId.Value });
                    }
                }

                if (q.Options == null) continue;

                foreach (var opt in q.Options.Select((o, idx) => new { Opt = o, Index = idx }))
                {
                    answerParams.Add(new { Id = Guid.NewGuid(), QuestionId = qid, Content = opt.Opt.Content, Right = opt.Opt.Right, Ordinal = opt.Index + 1, CreationTime = DateTime.Now, TenantId = tenantId.Value });
                }
            }
        }

        if (bankParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionBanks (Id, Name, Description, CreationTime, TenantId) VALUES (@Id, @Name, @Description, @CreationTime, @TenantId)", bankParams);
        }
        if (questionParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestions (Id, QuestionBankId, Title, QuestionType, Score, CreationTime, TenantId) VALUES (@Id, @QuestionBankId, @Title, @QuestionType, @Score, @CreationTime, @TenantId)", questionParams);
        }
        if (answerParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionAnswers (Id, QuestionId, Content, Right, Ordinal, CreationTime, TenantId) VALUES (@Id, @QuestionId, @Content, @Right, @Ordinal, @CreationTime, @TenantId)", answerParams);
        }
        if (knowledgepointParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionKnowledgePoints (QuestionId, KnowledgePointId, CreationTime, TenantId) VALUES (@QuestionId, @KnowledgePointId, @CreationTime, @TenantId)", knowledgepointParams);
        }
    }

    private async Task ClearDataAsync(IDbConnection connection, Guid tenantId)
    {
        await connection.ExecuteAsync("DELETE FROM AppQuestionBanks WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionAnswers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestions WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionKnowledgePoints WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPapers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPaperQuestionRules WHERE TenantId = @TenantId", new { TenantId = tenantId });
    }

    // DTOs for parsing questions.json
    public class QuestionsFile
    {
        public List<QuestionBankDto>? QuesiontBanks { get; set; }
    }

    public class QuestionBankDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<QuestionDto>? Questions { get; set; }
    }

    public class QuestionDto
    {
        public string? Title { get; set; }
        public int Type { get; set; }
        public string[] KnowledgePoints { get; set; } = [];
        public List<OptionDto>? Options { get; set; }
    }

    public class OptionDto
    {
        public string? Content { get; set; }
        public bool Right { get; set; }
    }

}