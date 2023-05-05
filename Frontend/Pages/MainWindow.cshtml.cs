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

    public List<string> SecurityChecks = new()
    {
        // "Company 1 | Date | Security Check Type | Progress",
        // "Company 2 | Date | Security Check Type | Progress"
    };

    //int index = HttpContext.Session.GetString("ProductsSortType") ?? DefaultSortType;
    //HttpContext.Session.SetString("OrdersSortType", OrdersSortType);


    public int SelectedSecurityCheckIndex;
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
        SelectedSecurityCheckIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0");
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
        var questionnaire = _db.Questionnaires.Select(x => x.QuestionnaireName).FirstOrDefault();
        var survey = _db.SurveyQuestions
            .Where(x => x.Questionnaire.QuestionnaireName == questionnaire)
            .Select(x => x)
            .FirstOrDefault();
        if (questionnaire == null || survey == null)
            return new RedirectToPageResult("MainWindow", new { ErrorText = "No Security Check found" });

        survey.CreatedDate = DateTime.Now;
        _db.CustomerSurveys.Add(new CustomerSurvey()
        {
            SurveyQuestion = survey,
        });
        _db.SaveChanges();
        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnPostSecurityCheckListChanged(string selectedItem)
    {
        Initialize();
        for (int i = 0; i < SecurityChecks.Count; i++)
        {
            if (SecurityChecks[i].Contains(selectedItem))
            {
                HttpContext.Session.SetString("SelectedSecurityCheckIndex", i.ToString());
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