namespace WeData.QuestionFlow.Engine;

public abstract class QuestionStrategy
{
    protected int Steps { get; set; }
    public Dictionary<string, IQuestionAnswer> QuestionAnswers { get; protected set; }
    public Dictionary<string, QuestionRule> QuestionRules { get; protected set; }

    public void Initialize(IEnumerable<QuestionRule> questionRules, IEnumerable<IQuestionAnswer> questionAnswers)
    {
        //todo: check duplicate rules and questions

        //todo: check question answer, only one option's selected is true

        QuestionRules = new Dictionary<string, QuestionRule>(StringComparer.InvariantCultureIgnoreCase);
        QuestionAnswers = new Dictionary<string, IQuestionAnswer>(StringComparer.InvariantCultureIgnoreCase);
        foreach (var rule in questionRules)
        {
            QuestionRules.Add(rule.QuestionId, rule);
        }

        foreach (var answer in questionAnswers)
        {
            QuestionAnswers.Add(answer.QuestionId, answer);
        }
    }

    public abstract IQuestionResult Execute(string startQuestionId);
}