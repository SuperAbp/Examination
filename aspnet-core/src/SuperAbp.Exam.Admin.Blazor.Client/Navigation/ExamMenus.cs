namespace SuperAbp.Exam.Admin.Blazor.Client.Navigation;

public class ExamMenus
{
    private const string Prefix = "Exam";

    public const string Home = Prefix + ".Home";
    public const string QuestionManagement = Prefix + ".QuestionManagement";
    public const string Questions = QuestionManagement + ".Questions";
    public const string QuestionBanks = QuestionManagement + ".QuestionBanks";
    public const string PaperManagement = Prefix + ".PaperManagement";
    public const string Papers = PaperManagement + ".Papers";
    public const string ExamManagement = Prefix + ".ExamManagement";
    public const string Exams = ExamManagement + ".Exams";
    public const string System = Prefix + ".System";
    public const string SystemAnnouncements = System + ".Announcements";
    public const string SystemAnnouncementCategories = System + ".AnnouncementCategories";
    public const string SystemKnowledgePoints = System + ".KnowledgePoints";
    public const string SystemNotifications = System + ".Notifications";
}
