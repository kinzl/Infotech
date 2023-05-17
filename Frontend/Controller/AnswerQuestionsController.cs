namespace Questionnaire_Frontend.Controller;
[Route("[controller]/[action]")]
[ApiController]
public class AnswerQuestionsController
{
    private SecurityCheckContext _db;
    public AnswerQuestionsController(SecurityCheckContext db)
    {
        _db = db;
    }

    [HttpGet("{jsonString}")]
    public List<QuestionDto> DeserializeJsonString(string jsonString)
    {
        Console.WriteLine("Received JSON: \n" + jsonString);
        List<QuestionDto> dataList = JsonSerializer.Deserialize<List<QuestionDto>>(jsonString) ?? new List<QuestionDto>();

        var lastCustomerSurvey = _db.CustomerSurveys.Select(x => x).OrderBy(x => x.CustomerSurveyId).Last();
        var survey = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Include(x => x.Question)
            .Include(x => x.Question.Answers)
            .Include(x => x.Question.Category)
            .Where(x => x.Questionnaire.QuestionnaireName == dataList.Select(y => y.Questionnaire).First())
            .Where(x => x.CustomerSurveyId == lastCustomerSurvey.CustomerSurveyId)
            .Select(x => x)
            .ToList();

        CriticismType criticismTypeRisk = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Risk").Select(x => x).Single();
        CriticismType criticismTypeRecommendation = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Recommendation").Select(x => x).Single();
        CriticismType criticismTypeReason = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Reason").Select(x => x).Single();

        for (int i = 0; i < survey.Count; i++)
        {

            survey[i].Question.Answers.Select(x => x).ToList()[0].IsChecked = dataList[i].Answer.NotAnswered.Selected;
            survey[i].Question.Answers.Select(x => x).ToList()[1].IsChecked = dataList[i].Answer.AnswerZero.Selected;
            survey[i].Question.Answers.Select(x => x).ToList()[2].IsChecked = dataList[i].Answer.AnswerOne.Selected;
            survey[i].Question.Answers.Select(x => x).ToList()[3].IsChecked = dataList[i].Answer.AnswerTwo.Selected;
            survey[i].Question.Answers.Select(x => x).ToList()[4].IsChecked = dataList[i].Answer.AnswerThree.Selected;


            survey[i].Question.Category.CategoryText = dataList[i].Category;
            survey[i].Question.Criticisms.Add(new Criticism()
            {
                CriticismType = criticismTypeReason,
                CriticismText = dataList[i].Reason,
            });
            survey[i].Question.Criticisms.Add(new Criticism()
            {
                CriticismType = criticismTypeRisk,
                CriticismText = dataList[i].Risk,
            });
            survey[i].Question.Criticisms.Add(new Criticism()
            {
                CriticismType = criticismTypeRecommendation,
                CriticismText = dataList[i].Recommendation,
            });

        }
        _db.SaveChanges();

        return dataList;
    }

}