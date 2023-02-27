using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class ChangePassword : PageModel
{
    public void OnGet()
    {
        
    }

    public void OnGetRedirectMainWindow()
    {
        Response.Redirect("MainWindow");
    }
}