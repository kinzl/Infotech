using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class ChangePassword : PageModel
{
    public string? ErrorText;
    public void OnGet(string? errorText)
    {
        ErrorText = errorText;
    }

    public void OnGetRedirectMainWindow()
    {
        Response.Redirect("MainWindow");
    }

    public IActionResult OnPostChangePassword()
    {
        return new RedirectToPageResult("ChangePassword", new{ErrorText = "Possible Error Message"});
    }
}