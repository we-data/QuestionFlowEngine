using Xunit.Abstractions;

namespace WeData.QuestionFlow.Engine
{
    public class QuestionFlowEngineTest
    {
        private readonly ITestOutputHelper output;
        private const string QUESTIONFLOW_NAME = "Sample1";
        public QuestionFlowEngineTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void QuestionFlowEngine_Can_Load_Rules_From_JsonFile()
        {
            var questionFlow = CreateQuestionFlowEngine();
            Assert.True(questionFlow.ContainsQuestionFlow(QUESTIONFLOW_NAME));
        }


        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "successAnswer1")]
        public async void QuestionFlowEngine_Can_Execute_Success_Answer(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            Assert.Equal(2, questionAnswers.Length);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            Assert.True(result.Type == QuestionActionType.Success);
        }

        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "firstAnswer")]
        public async Task QuestionFlowEngine_Can_Get_Following_Question1Async(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            Assert.Single(questionAnswers);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            Assert.True(result.Type == QuestionActionType.Next);
            //Question 2: 73fbba56-4a1a-49ab-b63b-a37675a0ef30
            var act = result.Actions.Where(action => action.FollowQuestionId == "73fbba56-4a1a-49ab-b63b-a37675a0ef30");
            Assert.Single(act);
        }

        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "continueAnswer")]
        public async Task QuestionFlowEngine_Can_Get_Following_Question2Async(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            Assert.Equal(2, questionAnswers.Length);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            Assert.True(result.Type == QuestionActionType.Next);
            Assert.Single(result.Actions.Select(action => action.FollowQuestionId == "0dd1464a-ee89-4b5c-9ea3-d42f84b48774"));
        }

        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "multipleOptions")]
        public async Task QuestionFlowEngine_Can_Execute_MutipleOptionsAsync(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            output.WriteLine(result.Type.ToString());
            Assert.True(result.Type == QuestionActionType.Next);
            Assert.Single(result.Actions.Select(action => action.FollowQuestionId == "8ef7396f-96bd-4a6a-b842-6f56af91fff9"));
        }

        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "stopAnswer")]
        public async Task QuestionFlowEngine_Can_Execute_Stop_AnswerAsync(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            Assert.True(result.Type == QuestionActionType.Stop);
        }

        [Theory]
        [QuestionAnswerFileData("QuestionFlow/questionAnswers.json", "failureAnswer")]
        public async Task QuestionFlowEngine_Can_Get_FailureAsync(params QuestionAnswer[] questionAnswers)
        {
            Assert.NotNull(questionAnswers);
            var questionFlow = CreateQuestionFlowEngine();

            var result = await questionFlow.ExecuteAsync(QUESTIONFLOW_NAME, questionAnswers);
            output.WriteLine(result.Type.ToString());
            Assert.True(result.Type == QuestionActionType.Failure);
        }

        [Fact]
        public void QuestionFlowEngine_Can_Load_Mutiple_QuestionFlow()
        {
            var questionFlow = new QuestionFlowEngine();

            string fileName = "QuestionFlow/questionRules.json";
            string jsonString = File.ReadAllText(fileName);
            questionFlow.AddQuestionFlow(jsonString);

            fileName = "QuestionFlow/questionFlow1.json";
            jsonString = File.ReadAllText(fileName);
            questionFlow.AddQuestionFlow(jsonString);
            Assert.True(questionFlow.ContainsQuestionFlow(QUESTIONFLOW_NAME));
            Assert.True(questionFlow.ContainsQuestionFlow("questionFlow1"));
        }

        private IQuestionFlowEngine CreateQuestionFlowEngine()
        {
            var questionFlow = new QuestionFlowEngine();

            string fileName = "QuestionFlow/QuestionFlow1.json";
            string jsonString = File.ReadAllText(fileName);
            questionFlow.AddQuestionFlow(jsonString);

            return questionFlow;
        }
    }
}
