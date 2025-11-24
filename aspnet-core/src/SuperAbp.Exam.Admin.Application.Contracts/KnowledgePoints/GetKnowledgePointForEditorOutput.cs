using SuperAbp.Exam.KnowledgePoints;
using System;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

public class GetKnowledgePointForEditorOutput
{
    public Guid? ParentId { get; set; }

    [Required]
    [MaxLength(KnowledgePointConsts.MaxNameLength)]
    public required string Name { get; set; }
}