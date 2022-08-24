namespace WeData.QuestionFlow.Engine;

public interface IQuestionOption
{
    int OptionNumber { get; set; }
    string ResponseText { get; set; }
    bool Selected { get; set; }
}
