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
    private string Username;
    public string? ErrorText;

    public ChangePassword(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet(string? errorText)
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        Username = HttpContext.User.Identities.ToList().First().Name;
        Console.WriteLine("Change Password Username: " + Username);
        ErrorText = errorText;
        return null;
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
