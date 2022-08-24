using Newtonsoft.Json;

namespace WeData.QuestionFlow.Engine;

public class QuestionFlowEngine : IQuestionFlowEngine
{
    private readonly QuestionRulesCache _questionRulesCache;
    public IList<QuestionRule> QuestionRules { get; protected set; }
    public IList<IQuestionAnswer> Questions { get; protected set; }
    private QuestionStrategy _questionStrategy;

    public QuestionFlowEngine()
    {
        _questionRulesCache = new QuestionRulesCache();
        _questionStrategy = new SingleResultStrategy();
    }
    public void AddQuestionFlow(params string[] jsonStrings)
    {
        var questionFlow = jsonStrings.Select(item => JsonConvert.DeserializeObject<QuestionFlow>(item)).ToArray();
        AddQuestionFlow(questionFlow);
    }

    public async Task<IQuestionResult> ExecuteAsync(string questionFlowName, IQuestionAnswer[] questionAnswers)
    {
        var questionFlow = _questionRulesCache.GetQuestionFlow(questionFlowName);
        if (questionFlow == null)
        {
            questionFlow = await LoadQuestionFlow(questionFlowName);
            if (questionFlow == null)
            {
                throw new Exception();
            }
            AddQuestionFlow(questionFlow);
        }
        //todo: check questionAnswers is not null or empty
        _questionStrategy.Initialize(questionFlow.Rules, questionAnswers);
        return _questionStrategy.Execute(questionAnswers[0].QuestionId);
    }

    //Todo: will remove this method in the future, engine should not depend on repository, workflow should be load outside of engine
    private Task<QuestionFlow> LoadQuestionFlow(string questionFlowName)
    {
        throw new NotImplementedException();
    }

    public void AddQuestionFlow(params QuestionFlow[] questionFlows)
    {
        try
        {
            foreach (var questionFlow in questionFlows)
            {
                if (!_questionRulesCache.ContainsQuestionFlow(questionFlow.QuestionFlowName))
                {
                    _questionRulesCache.AddOrUpdateQuestionFlow(questionFlow.QuestionFlowName, questionFlow);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool ContainsQuestionFlow(string questionFlowName)
    {
        return _questionRulesCache.ContainsQuestionFlow(questionFlowName);
    }

    public void ClearQuestionFlows()
    {
        _questionRulesCache.Clear();
    }

    public void RemoveQuestionFlow(params string[] questionFlowNames)
    {
        foreach (var name in questionFlowNames)
        {
            _questionRulesCache.Remove(name);
        }
    }

    public List<string> GetAllRegisteredQuestionFlowNames()
    {
        return _questionRulesCache.GetAllQuestionFlowNames();
    }

    public void AddOrUpdateQuestionFlow(params QuestionFlow[] questionFlows)
    {
        try
        {
            foreach (var questionFlow in questionFlows)
            {
                _questionRulesCache.AddOrUpdateQuestionFlow(questionFlow.QuestionFlowName, questionFlow);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
