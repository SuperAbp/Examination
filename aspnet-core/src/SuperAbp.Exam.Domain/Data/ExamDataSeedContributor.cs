using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.Data
{
    internal class ExamDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IQuestionBankRepository _questionRepoRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public ExamDataSeedContributor(ICurrentTenant currentTenant, IGuidGenerator guidGenerator, IQuestionBankRepository questionRepoRepository)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _questionRepoRepository = questionRepoRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _questionRepoRepository.GetCountAsync() > 0)
                {
                    return;
                }
                var books = new List<QuestionBank>
                {
                    new QuestionBank(
                        id: _guidGenerator.Create(),
                        DateTime.Now.Year +"年上学期期中考试"),
                    new QuestionBank(
                        id: _guidGenerator.Create(),
                        DateTime.Now.Year +"年上学期期末考试")
                };

                await _questionRepoRepository.InsertManyAsync(books);
            }
        }
    }
}