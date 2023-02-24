using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class MainWindow : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<int> list = new() { 1, 2, 3, 4, 5 };

    public MainWindow(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public void OnGetRedirectShowExistingData()
    {
        Response.Redirect("AnswerQuestions");

    }

    public IActionResult OnPostCreateNewSecurityCheck(string questionnaireName)
    {
        // Response.Redirect("AnswerQuestions");
        return new RedirectToPageResult("AnswerQuestions");
        //db add new secu 
    }
    public void OnGetRedirectToUpdateSecurityCheck()
    {
        Response.Redirect("UpdateSecurityCheck");
    }
}