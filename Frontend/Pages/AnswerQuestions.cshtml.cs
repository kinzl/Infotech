namespace Questionnaire_Frontend.Pages;

public class AnswerQuestions : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;


    public List<string> SecurityCheckType = new()
    {
        // "Security Check Light",
        // "Security Check Extended"
    };

    public List<string> Question = new()
    {
        // "Question 1",
        // "Question 2", 
        // "Question 3"
    };

    public List<QuestionDto> AllQuestionsAndAnswers = new()
    {
        new QuestionDto()
        {
            Question = "Frage nummero one",
            Category = "01 Kategorie 1",
            Criticality = "Kritisch",
            Answer = new AnswerDto()
            {
                NotAnswered = new DetailAnswerDto()
                {
                    Answertext = "n.A.",
                    Selected = true
                },
                AnswerZero = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 0",
                    Selected = false
                },
                AnswerOne = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 1",
                    Selected = false
                },
                AnswerTwo = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 2",
                    Selected = false
                },
                AnswerThree = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 3",
                    Selected = false
                },
            }
        },
        new QuestionDto()
        {
            Question = "Frage nummero dos",
            Category = "02 Kategorie 2",
            Criticality = "Kritisch",
            Answer = new AnswerDto()
            {
                NotAnswered = new DetailAnswerDto()
                {
                    Answertext = "n.A.",
                    Selected = true
                },
                AnswerZero = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 0",
                    Selected = false
                },
                AnswerOne = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 1",
                    Selected = false
                },
                AnswerTwo = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 2",
                    Selected = false
                },
                AnswerThree = new DetailAnswerDto()
                {
                    Answertext = "Antwort nummer 3",
                    Selected = false
                },
            }
        }
    };

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
        Initialize();
        return null;
    }

    private void Initialize()
    {
        SecurityCheckType = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        SelectedSecurityCheck = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheck") ?? "0");
        CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";

        AllQuestionsAndAnswers = _db.SurveyQuestions
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Include(x => x.Question.Category)
            .Include(x => x.Question.Answers)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => new QuestionDto()
            {
                Category = x.Question.Category.CategoryText,
                Question = x.Question.QuestionText,
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

        var thisSurvey = _db.CustomerSurveys
            .Include(x => x.SurveyQuestion)
            .ThenInclude(x => x.Questionnaire)
            .Select(x => x)
            .OrderBy(x => x.CustomerSurveyId)
            .Last();
        var survey = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => x)
            .FirstOrDefault();

        thisSurvey.SurveyQuestion = survey;
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