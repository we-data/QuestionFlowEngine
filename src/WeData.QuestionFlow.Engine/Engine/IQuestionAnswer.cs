namespace WeData.QuestionFlow.Engine;

public interface IQuestionAnswer
{
    string QuestionId { get; set; }
    IList<IQuestionOption> Options { get; set; }
}
