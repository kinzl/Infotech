using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class MainWindow : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public List<string> SecurityChecks = new();

    public int SelectedSecurityCheckIndexMainWindow;
    public string ErrorText;

    public MainWindow(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet(string errorText)
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        //Username = HttpContext.User.Identities.Select(x => x.Name).ToList() //.First

        Console.WriteLine("User " + HttpContext.User.Identities.ToList().First().Name + " Signed in");

        ErrorText = errorText;
        Initialize();
        return null;
    }

    private void Initialize()
    {
        SelectedSecurityCheckIndexMainWindow =
            int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndexMainWindow") ?? "0");
        SecurityChecks = _db.CustomerSurveys
            .Include(x => x.SurveyQuestion)
            .ThenInclude(x => x.Questionnaire)
            .Select(x => x.SurveyQuestion)
            .ToList()
            .IsNullOrEmpty()
            ? new List<string>()
            : _db.CustomerSurveys.Select(x => x.ToDataString()).ToList();
    }

    public IActionResult OnPostNewSecurityCheck()
    {
        HttpContext.Session.SetString("SelectedSecurityCheck", "0");
        try
        {
            var questionnaire = _db.Questionnaires.Select(x => x.QuestionnaireName).FirstOrDefault();
            var survey = _db.SurveyQuestions
                .Include(x => x.Question)
                .Include(x => x.Questionnaire)
                .Include(x => x.Question.Answers)
                .Include(x => x.Question.Category)
                .Where(x => x.Questionnaire.QuestionnaireName == questionnaire)
                .Where(x => x.CustomerSurvey.CustomerSurveyId == null)
                .Select(x => x)
                .ToList();
            if (questionnaire == null || survey == null)
                return new RedirectToPageResult("MainWindow",
                    new
                    {
                        ErrorText = "The first Security Check might have no questions or there is no security check"
                    });

            var questions = survey.Select(x => x.Question).ToList();

            var customerSurvey = new CustomerSurvey()
            {
                CreatedDate = DateTime.Now,
            };
            for (int i = 0; i < survey.Count; i++)
            {
                var answers = questions[i].Answers.ToList();
                survey[i].SurveyQuestionId = 0;
                survey[i].CustomerSurvey = customerSurvey;
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
        catch (Exception e)
        {
            return new RedirectToPageResult("MainWindow",
                new { ErrorText = "The same question might be in the question Pool" });
        }
    }

    public async Task<IActionResult> OnPostLogout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new RedirectToPageResult("Index");
    }

    public IActionResult OnPostSecurityCheckListChanged(string selectedItem)
    {
        Initialize();
        for (int i = 0; i < SecurityChecks.Count; i++)
        {
            if (SecurityChecks[i].Contains(selectedItem))
            {
                HttpContext.Session.SetString("SelectedSecurityCheckIndexMainWindow", i.ToString());
                break;
            }
        }

        return new RedirectToPageResult("MainWindow");
    }

    public IActionResult OnPostOpenSelectedCheck()
    {
        return new RedirectToPageResult("AnswerQuestionsExtended");
    }

    public IActionResult OnGetRedirectToUpdateSecurityCheck()
    {
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnGetRedirectChangePassword()
    {
        return new RedirectToPageResult("ChangePassword");
    }
}