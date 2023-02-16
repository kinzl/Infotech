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

    public void OnPostCreateNewSecurityCheck(string questionnaireName)
    {
        Response.Redirect("AnswerQuestions");
        //db add new secu 
    }
}