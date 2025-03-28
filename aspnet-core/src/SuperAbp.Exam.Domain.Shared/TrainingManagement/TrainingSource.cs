using Ardalis.SmartEnum;

namespace SuperAbp.Exam.TrainingManagement;

/// <summary>
/// 训练类型
/// </summary>
public sealed class TrainingSource : SmartEnum<TrainingSource>
{
    public static readonly TrainingSource QuestionRepository = new TrainingSource(nameof(QuestionRepository), 1);
    public static readonly TrainingSource Favorite = new TrainingSource(nameof(Favorite), 2);

    public TrainingSource(string name, int value) : base(name, value)
    {
    }
}