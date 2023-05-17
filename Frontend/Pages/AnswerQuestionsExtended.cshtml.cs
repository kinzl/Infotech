using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class AnswerQuestionsExtendedModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public AnswerQuestionsExtendedModel(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public List<QuestionDto> AllQuestionsAndAnswers = new();

    public string? CompanyName { get; set; } 
    public string? SecurityCheckType { get; set; } 

    public IActionResult OnGet()
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        Initialize();
        return null;
    }

    private void Initialize()
    {
        // SelectedSecurityCheckIndex + 1 = primaryKey
        int index = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0") + 1;
        AllQuestionsAndAnswers = _db.SurveyQuestions
            .Include(x => x.CustomerSurvey)
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Include(x => x.Question.Answers)
            .Include(x => x.Question.Category)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => new QuestionDto()
            {
                CompanyName = x.CustomerSurvey.CompanyName,
                Category = x.Question.Category.CategoryText,
                Question = x.Question.QuestionText,
                Criticality = x.Question.Criticality.CriticalityText,
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
        CompanyName = _db.SurveyQuestions
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => x.CustomerSurvey.CompanyName)
            .First() ?? "";
        SecurityCheckType = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => x.Questionnaire.QuestionnaireName)
            .First();
    }

    public void OnPostDownloadSecurityCheck()
    {
        Console.WriteLine("Download");
    }

    public IActionResult OnPostRedirectMainWindow()
    {
        return new RedirectToPageResult("MainWindow");
    }
}