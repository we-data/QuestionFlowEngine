namespace WeData.QuestionFlow.Engine;

public class QuestionRule
{
    public string RuleName { get; set; }
    public string QuestionId { get; set; }
    public Dictionary<string, object> Properties { get; set; }
    public List<QuestionAction> Actions { get; set; }
}
