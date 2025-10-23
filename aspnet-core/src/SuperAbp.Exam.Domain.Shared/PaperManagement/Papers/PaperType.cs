using Ardalis.SmartEnum;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperType : SmartEnum<PaperType>
{
    public static readonly PaperType Fixed = new PaperType(nameof(Fixed), 0);
    public static readonly PaperType Random = new PaperType(nameof(Random), 1);

    public PaperType(string name, int value) : base(name, value)
    {
    }
}