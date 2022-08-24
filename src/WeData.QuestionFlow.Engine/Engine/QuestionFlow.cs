namespace WeData.QuestionFlow.Engine;

public class QuestionFlow
{
    public string QuestionFlowName { get; set; }
    /// <summary>
    /// Make the multiple questionflows' rules can be combined together.
    /// </summary>
    public IEnumerable<string> QuestionFlowsToInject { get; set; }
    public IEnumerable<QuestionRule> Rules { get; set; }
}
