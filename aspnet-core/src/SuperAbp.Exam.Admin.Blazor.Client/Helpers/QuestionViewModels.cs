using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.Blazor.Client.Helpers;

public sealed record QuestionOptionViewModel(Guid Id, string Content, bool? Right);

public sealed record QuestionNumberSection(string Title, decimal? TotalScore, IReadOnlyList<QuestionNumberItem> Questions);

public sealed record QuestionNumberItem(Guid Id, decimal? Score);

