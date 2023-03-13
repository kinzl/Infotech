using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class ChangePassword : PageModel
{
    public string? ErrorText = "Test";
    public void OnGet()
    {
        
    }

    public void OnGetRedirectMainWindow()
    {
        Response.Redirect("MainWindow");
    }

    public IActionResult OnPostChangePassword()
    {
        return null;
    }
}