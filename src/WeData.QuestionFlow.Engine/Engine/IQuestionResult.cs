namespace WeData.QuestionFlow.Engine;

public interface IQuestionResult
{
    QuestionActionType Type { get; set; }
    string Message { get; set; }
    IList<QuestionAction> Actions { get; set; }
}
