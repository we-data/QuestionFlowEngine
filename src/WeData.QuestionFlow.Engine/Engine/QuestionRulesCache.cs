using System.Collections.Concurrent;

namespace WeData.QuestionFlow.Engine;

internal class QuestionRulesCache
{
    private readonly ConcurrentDictionary<string, (QuestionFlow, long)> _questionFlow = new ConcurrentDictionary<string, (QuestionFlow, long)>();
    public QuestionRulesCache()
    {
    }
    public bool ContainsQuestionFlow(string questionflowName)
    {
        return _questionFlow.ContainsKey(questionflowName);
    }
    public List<string> GetAllQuestionFlowNames()
    {
        return _questionFlow.Keys.ToList();
    }
    public void AddOrUpdateQuestionFlow(string questionflowName, QuestionFlow questionFlow)
    {
        long ticks = DateTime.UtcNow.Ticks;
        _questionFlow.AddOrUpdate(questionflowName, (questionFlow, ticks), (k, v) => (questionFlow, ticks));
    }

    public QuestionFlow GetQuestionFlow(string questionflowName)
    {
        if (_questionFlow.TryGetValue(questionflowName, out (QuestionFlow questionFlow, long tick) QuestionflowsObj))
        {
            var questionFlow = QuestionflowsObj.questionFlow;
            if (questionFlow.QuestionFlowsToInject?.Any() == true)
            {
                if (questionFlow.Rules == null)
                {
                    questionFlow.Rules = new List<QuestionRule>();
                }
                foreach (string s in questionFlow.QuestionFlowsToInject)
                {
                    var injectedFlow = GetQuestionFlow(s);
                    if (injectedFlow == null)
                    {
                        throw new Exception($"Could not find injected QuestionFlow: {s}");
                    }

                    questionFlow.Rules = questionFlow.Rules.Concat(injectedFlow.Rules).ToList();
                }
            }
            return questionFlow;
        }
        else
        {
            return null;
        }
    }
    public void Remove(string questionFlowName)
    {
        _questionFlow.TryRemove(questionFlowName, out var questionFlowObj);
    }
    public void Clear()
    {
        _questionFlow.Clear();
    }
}
