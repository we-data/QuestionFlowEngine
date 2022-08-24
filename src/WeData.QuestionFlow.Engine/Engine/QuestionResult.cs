namespace WeData.QuestionFlow.Engine;

public class QuestionResult : IQuestionResult
{
    public QuestionActionType Type { get; set; }
    public string Message { get; set; }
    public IList<QuestionAction> Actions { get; set; }

    public QuestionResult()
    {
        Type = QuestionActionType.Success;
        Actions = new List<QuestionAction>();
    }
}
