namespace WeData.QuestionFlow.Engine;

public interface IQuestionFlowEngine
{
    void AddQuestionFlow(params QuestionFlow[] questionFlows);
    void AddQuestionFlow(params string[] jsonStrings);
    void ClearQuestionFlows();
    void RemoveQuestionFlow(params string[] questionFlowNames);
    bool ContainsQuestionFlow(string questionFlowName);
    List<string> GetAllRegisteredQuestionFlowNames();
    void AddOrUpdateQuestionFlow(params QuestionFlow[] questionFlows);
    Task<IQuestionResult> ExecuteAsync(string questionFlowName, IQuestionAnswer[] questionAnswers);
}
