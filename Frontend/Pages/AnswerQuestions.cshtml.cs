namespace Questionnaire_Frontend.Pages;

public class AnswerQuestions : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;


    public List<string> SecurityCheckType = new();

    public List<string> Question = new();

    public List<QuestionDto> AllQuestionsAndAnswers = new();

    public int SelectedSecurityCheck;
    public string CompanyName = "";

    public AnswerQuestions(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public List<string> Criticality { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        try
        {
            Initialize();
        }
        catch (Exception ex)
        {
            return new RedirectToPageResult("MainWindow", new { ErrorText = "No Check found" });
        }

        return null;
    }

    private void Initialize()
    {
        SecurityCheckType = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        SelectedSecurityCheck = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheck") ?? "0");
        CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";


        var lastCustomerSurvey = _db.CustomerSurveys.Select(x => x).OrderBy(x => x.CustomerSurveyId).Last();
        AllQuestionsAndAnswers = _db.SurveyQuestions
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Include(x => x.Question.Category)
            .Include(x => x.Question.Answers)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Where(x => x.CustomerSurveyId == lastCustomerSurvey.CustomerSurveyId)
            .Select(x => new QuestionDto()
            {
                Category = x.Question.Category.CategoryText,
                Question = x.Question.QuestionText,
                Questionnaire = x.Questionnaire.QuestionnaireName,
                Reason = "",
                Recommendation = "",
                Risk = "",
                Answer = new AnswerDto()
                {
                    AnswerZero = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 0).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 0).Select(x => x.IsChecked).Single(),
                        Points = 0
                    },
                    AnswerOne = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 1).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 1).Select(x => x.IsChecked).Single(),
                        Points = 1
                    },
                    AnswerTwo = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 2).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 2).Select(x => x.IsChecked).Single(),
                        Points = 2
                    },
                    AnswerThree = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 3).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 3).Select(x => x.IsChecked).Single(),
                        Points = 3
                    },
                    NotAnswered = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == -1).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == -1).Select(x => x.IsChecked).Single(),
                        Points = -1
                    }
                }
            })
            .ToList();

        Criticality = _db.Criticalities
            .Select(x => x.CriticalityText)
            .ToList();
    }

    public IActionResult OnPostSecurityCheckTypeChanged(string securityCheck)
    {
        Initialize();
        for (int i = 0; i < SecurityCheckType.Count; i++)
        {
            if (SecurityCheckType[i] == securityCheck)
            {
                SelectedSecurityCheck = i;
                HttpContext.Session.SetString("SelectedSecurityCheck", i.ToString());
                break;
            }
        }

        var lastCustomerSurvey = _db.CustomerSurveys.Select(x => x).OrderBy(x => x.CustomerSurveyId).Last();

        var oldSurvey = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Include(x => x.Question)
            .Include(x => x.Question.Answers)
            .Include(x => x.Question.Category)
            .Include(x => x.CustomerSurvey)
            .Where(x => x.CustomerSurveyId == lastCustomerSurvey.CustomerSurveyId)
            .Select(x => x)
            .ToList();

        var newQuestionnaire = _db.Questionnaires
            .Where(x => x.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck]).First();

        var newQuestions = _db.SurveyQuestions
            .Include(x => x.CustomerSurvey)
            .Include(x => x.Question)
            .Include(x => x.Question.Answers)
            .Include(x => x.Question.Category)
            .Where(x => x.CustomerSurveyId == null)
            .Where(x => x.Questionnaire.QuestionnaireId != null)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => x.Question)
            .ToList();

        foreach (var item in oldSurvey)
        {
            _db.SurveyQuestions.Remove(item);
        }

        foreach (var item in newQuestions)
        {
            _db.SurveyQuestions.Add(new SurveyQuestion()
            {
                Questionnaire = newQuestionnaire,
                Question = item,
                CustomerSurveyId = lastCustomerSurvey.CustomerSurveyId,
                
            });
        }
        _db.SaveChanges();

        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnGetRedirectToAnswerQuestionsExtended()
    {
        return new RedirectToPageResult("AnswerQuestionsExtended");
    }

    public IActionResult OnPostSubmitCompanyName(string companyName)
    {
        var thisSurvey = _db.CustomerSurveys
            .Select(x => x)
            .OrderBy(x => x.CustomerSurveyId)
            .LastOrDefault();
        thisSurvey.CompanyName = companyName;
        _db.SaveChanges();
        HttpContext.Session.SetString("CompanyName", companyName);
        return new RedirectToPageResult("AnswerQuestions");
    }

    public void OnGetSave()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }
}