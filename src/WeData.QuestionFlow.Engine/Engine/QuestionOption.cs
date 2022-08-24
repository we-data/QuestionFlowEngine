namespace WeData.QuestionFlow.Engine;

public class QuestionOption : IQuestionOption
{
    public int OptionNumber { get; set; }
    public string ResponseText { get; set; }
    public bool Selected { get; set; }
    public List<string> Description { get; set; }
}