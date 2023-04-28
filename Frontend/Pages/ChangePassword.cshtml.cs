using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
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

    public IActionResult OnGetRedirectMainWindow()
    {
        return new RedirectToPageResult("MainWindow");
    }

    public IActionResult OnPostChangePassword(string? oldPassword, string? newPassword, string? newPasswordRepeated)
    {
        if(oldPassword.IsNullOrEmpty() || oldPassword.IsNullOrEmpty() || newPasswordRepeated.IsNullOrEmpty())
            return new RedirectToPageResult("ChangePassword", new{ErrorText = "Some fields were empty"});
        
        //ToDo: Convert password to salt and hash and compare them
        
        return new RedirectToPageResult("ChangePassword", new{ErrorText = "Error"});
    }
}
