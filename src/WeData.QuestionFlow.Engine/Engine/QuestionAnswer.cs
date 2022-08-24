namespace WeData.QuestionFlow.Engine;

public class QuestionAnswer : IQuestionAnswer
{
    public QuestionAnswer(string id, IEnumerable<QuestionOption> options)
    {
        QuestionId = id;
        Options = options.ToList<IQuestionOption>();
    }
    public string QuestionId { get; set; }
    public IList<IQuestionOption> Options { get; set; }
}
