namespace WeData.QuestionFlow.Engine;

public class SingleResultStrategy : QuestionStrategy
{
    public SingleResultStrategy()
    {
    }

    public override IQuestionResult Execute(string startQuestionId)
    {
        Steps = 0;
        var result = new QuestionResult();
        var action = GetAction(startQuestionId);

        QuestionAction lastAction = null;
        while (action != null)
        {
            lastAction = action;
            if (action.Type == QuestionActionType.Continue)
            {
                action = GetAction(action.FollowQuestionId);
            }
            else
            {
                break;
            }
        }

        if (lastAction == null)
        {
            result.Type = QuestionActionType.Failure;
        }
        else
        {
            result.Type = lastAction.Type;
            result.Message = lastAction.Message;
            result.Actions.Add(lastAction);
        }
        return result;
    }

    public QuestionAction GetAction(string questionId)
    {
        Steps++;
        if (QuestionRules.ContainsKey(questionId))
        {
            var questionRule = QuestionRules[questionId];
            if (QuestionAnswers.ContainsKey(questionRule.QuestionId))
            {
                var question = QuestionAnswers[questionRule.QuestionId];
                foreach (IQuestionOption option in question.Options)
                {
                    if (option.Selected)
                    {
                        foreach (var action in questionRule.Actions)
                        {
                            if (action.OptionNumber == option.OptionNumber) return action;
                        }
                    }
                }
            }
            else if (QuestionAnswers.Count >= Steps)
                return new QuestionAction() { Type = QuestionActionType.Failure };
        }
        return null;
    }
}
