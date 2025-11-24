using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

public class KnowledgePointNodeDto : EntityDto<Guid>
{
    public required string Name { get; set; }

    public List<KnowledgePointNodeDto> Children { get; set; }
}