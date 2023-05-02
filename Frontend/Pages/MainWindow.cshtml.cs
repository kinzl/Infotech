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


    public int SelectedSecurityCheckIndex = 0;

    public MainWindow(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet()
    {
        Initialize();
    }

    private void Initialize()
    {
        SelectedSecurityCheckIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0");
        SecurityChecks = _db.CustomerSurveys.Include(x => x.SurveyQuestion).Select(x => x.SurveyQuestion.ToDataString()).ToList().IsNullOrEmpty() 
            ? _db.CustomerSurveys.Select(x => x.SurveyQuestion.ToDataString()).ToList()
            : new List<string>();
    }

    public IActionResult OnPostNewSecurityCheck()
    {
        // _db.CustomerSurveys.Add(new CustomerSurvey() { SurveyQuestion = new SurveyQuestion()
        // {
        //      Question = _db.Questions.ToList(),
        //     
        // }});
        // _db.SaveChanges();
        return new RedirectToPageResult("AnswerQuestions");
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

    public IActionResult OnPostSecurityCheckListChanged(string selectedItem)
    {
        for (int i = 0; i < SecurityChecks.Count; i++)
        {
            if (SecurityChecks[i] == selectedItem)
            {
                SelectedSecurityCheckIndex = i;
                break;
            }
        }

        HttpContext.Session.SetString("SelectedSecurityCheckIndex", SelectedSecurityCheckIndex.ToString());
        return new RedirectToPageResult("MainWindow");
    }
}