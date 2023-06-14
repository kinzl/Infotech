using SecurityCheckDbLib;
using static iTextSharp.text.pdf.AcroFields;

namespace Questionnaire_Frontend.Pages;

public class AnswerQuestions : PageModel
{
    private readonly ILogger<AnswerQuestions> _logger;
    private SecurityCheckContext _db;


    public List<string> SecurityCheckType = new();

    public List<string> Question = new();

    public List<QuestionDto> AllQuestionsAndAnswers = new();

    public int SelectedSecurityCheck;
    public string CompanyName = "";
    public string TypeOfExecution = "";
    public string Participants = "";
    public string Scope = "";
    public string Classification = "";
    public string DocumentDistributor = "";
    public string ManagementSummary = "";

    public AnswerQuestions(ILogger<AnswerQuestions> logger, SecurityCheckContext db)
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
        _logger.LogInformation("AnswerQuestions Initialize");
        SecurityCheckType = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        SelectedSecurityCheck = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheck") ?? "0");
        CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
        TypeOfExecution = HttpContext.Session.GetString("TypeOfSurvey") ?? "";
        Participants = HttpContext.Session.GetString("Participant") ?? "";
        Scope = HttpContext.Session.GetString("Scope") ?? "";
        Classification = HttpContext.Session.GetString("Classification") ?? "";
        DocumentDistributor = HttpContext.Session.GetString("DocumentDistributor") ?? "";
        ManagementSummary = HttpContext.Session.GetString("ManagementSummaryText") ?? "";
        

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
        _logger.LogInformation("AnswerQuestions OnPostSecurityCheckTypeChanged");
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

        foreach (var item in oldSurvey)
        {
            _db.SurveyQuestions.Remove(item);
        }

        var survey = _db.SurveyQuestions
                .Include(x => x.Question)
                .Include(x => x.Questionnaire)
                .Include(x => x.Question.Answers)
                .Include(x => x.Question.Category)
                .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
                .Where(x => x.CustomerSurvey.CustomerSurveyId == null)
                .Select(x => x)
                .ToList();

        var questions = survey.Select(x => x.Question).ToList();


        for (int i = 0; i < survey.Count; i++)
        {
            var answers = questions[i].Answers.ToList();
            survey[i].SurveyQuestionId = 0;
            survey[i].CustomerSurvey = lastCustomerSurvey;
            var newQuestion = new Question()
            {
                QuestionId = 0,
                QuestionText = questions[i].QuestionText,
                Category = questions[i].Category,
                Answers = new List<Answer>()
                    {
                        new()
                        {
                            AnswerText = answers[0].AnswerText,
                            IsChecked = answers[0].IsChecked,
                            Points = answers[0].Points,
                        },
                        new()
                        {
                            AnswerText = answers[1].AnswerText,
                            IsChecked = answers[1].IsChecked,
                            Points = answers[1].Points,
                        },
                        new()
                        {
                            AnswerText = answers[2].AnswerText,
                            IsChecked = answers[2].IsChecked,
                            Points = answers[2].Points,
                        },
                        new()
                        {
                            AnswerText = answers[3].AnswerText,
                            IsChecked = answers[3].IsChecked,
                            Points = answers[3].Points,
                        },
                        new()
                        {
                            AnswerText = answers[4].AnswerText,
                            IsChecked = answers[4].IsChecked,
                            Points = answers[4].Points,
                        }
                    }

            };
            _db.Questions.Add(newQuestion);
            survey[i].Question = newQuestion;
            _db.SurveyQuestions.Add(survey[i]);
        }
        _db.SaveChanges();

        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnGetRedirectToAnswerQuestionsExtended()
    {
        _logger.LogInformation("AnswerQuestions OnGetRedirectToAnswerQuestionsExtended");
        return new RedirectToPageResult("AnswerQuestionsExtended");
    }

    public IActionResult OnPostSubmitCompanyName(string companyName, string typeOfExecution, string participants, string scope, string classification, string documentDistributor, string managementSummary)
    {
        _logger.LogInformation("AnswerQuestions OnPostSubmitCompanyName");
        var thisSurvey = _db.CustomerSurveys
            .Select(x => x)
            .OrderBy(x => x.CustomerSurveyId)
            .LastOrDefault();
        thisSurvey.CompanyName = companyName;
        thisSurvey.TypeOfSurvey = typeOfExecution;
        thisSurvey.Participant = participants;
        thisSurvey.Scope = scope;
        thisSurvey.Classification = classification;
        thisSurvey.DocumentDistributor = documentDistributor;
        thisSurvey.ManagementSummaryText = managementSummary;
        thisSurvey.DateOfExecution = DateTime.Now;

        _db.SaveChanges();
        HttpContext.Session.SetString("CompanyName", companyName);
        HttpContext.Session.SetString("TypeOfSurvey", typeOfExecution);
        HttpContext.Session.SetString("Participant", participants);
        HttpContext.Session.SetString("Classification", classification);
        HttpContext.Session.SetString("DocumentDistributor", companyName);
        HttpContext.Session.SetString("ManagementSummaryText", managementSummary);
        return new RedirectToPageResult("AnswerQuestions");
    }
}