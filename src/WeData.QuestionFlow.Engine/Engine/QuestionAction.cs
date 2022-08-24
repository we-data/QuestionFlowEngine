namespace WeData.QuestionFlow.Engine;

public class QuestionAction
{
    public QuestionActionType Type { get; set; }
    public int OptionNumber { get; set; }
    public string FollowQuestionId { get; set; }
    public string Message { get; set; }
}
