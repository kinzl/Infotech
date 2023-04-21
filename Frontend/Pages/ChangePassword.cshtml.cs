using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class ChangePassword : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;
    
    public string? ErrorText;

    public ChangePassword(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

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
