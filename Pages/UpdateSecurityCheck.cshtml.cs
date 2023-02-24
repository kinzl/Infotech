using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public List<string> list = new() { "Question 1","Question 2", "Question 3" };
    public void OnGet()
    {
        
    }

    public void OnGetRedirectToMainWindow()
    {
        Response.Redirect("MainWindow");
    }
}