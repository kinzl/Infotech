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
    private string BodyUsername;
    public string? ErrorText;

    public ChangePassword(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet(string? errorText)
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        BodyUsername = HttpContext.User.Identities.ToList().First().Name;
        Console.WriteLine("Change Password Username: " + BodyUsername);
        ErrorText = errorText;
        return null;
    }

    public IActionResult OnGetRedirectMainWindow()
    {
        return new RedirectToPageResult("MainWindow");
    }

    public IActionResult OnPostChangePassword(string? oldPassword, string? newPassword, string? newPasswordRepeated)
    {
        OnGet("");
        if (oldPassword.IsNullOrEmpty() || newPassword.IsNullOrEmpty() || newPasswordRepeated.IsNullOrEmpty())
            return new RedirectToPageResult("ChangePassword", new { ErrorText = "Some fields were empty" });

        PasswordEncryption pe = new PasswordEncryption();
        //Man muss auf Username zugreifen können um ihn danach zu vergleichen. 
        string oldhash = _db.UserNames.Where(x => x.Username == BodyUsername).Select(x => x.PasswordHash).First();
        Console.WriteLine("Hash von db= " + oldhash);
        string oldsalt = _db.UserNames.Where(x => x.Username == BodyUsername).Select(x => x.PasswordSalt).First();
        //byte[] bytes = Encoding.UTF8.GetBytes(oldsalt);

        if (pe.VerifyPassword(oldPassword, oldhash, oldsalt))
        {
            if (newPassword.Equals(newPasswordRepeated))
            {
                Console.WriteLine("NewPassword = " + newPassword);
                UserName newPw = _db.UserNames.Where(x => x.Username == BodyUsername).Select(x => x).First();

                var newhash = pe.HashPasword(newPassword, out var newsalt);
                Console.WriteLine("NewHash = " + newhash);
                Console.WriteLine("NewSalt = " + newsalt);
                newPw.PasswordHash = newhash;
                newPw.PasswordSalt = newsalt;
                _db.SaveChanges();
            }
            else
            {
                return new RedirectToPageResult("ChangePassword", new { ErrorText = "Repeated Password does not match" });
            }
            return new RedirectToPageResult("MainWindow", new { ErrorText = "Password Changed" });
        }
        else
        {
            return new RedirectToPageResult("ChangePassword", new { ErrorText = "Old Password does not match" });
        }
        //ToDo: Convert password to salt and hash and compare them

        //return new RedirectToPageResult("ChangePassword", new { ErrorText = "Error" });
    }
}
