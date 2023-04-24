using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Questionnaire_Frontend.Dto;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    public string? ErrorText;
    public string Username = "Admin";
    public string Password = "admin";

    public string InputUsername { get; set; }
    public string InputPassword { get; set; }

    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public IndexModel(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;

        // db.Database.EnsureDeleted();
        // db.Database.EnsureCreated();
        try
        {
            AddUserNames();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    private void AddUserNames()
    {
        UserName admin = new UserName
        {
            IsAdmin = false,
            Username = "kinzl",
            PasswordHash = "s45g5",
            PasswordSalt = "df45"
        };
        _db.UserNames.Add(admin);
        _db.SaveChanges();
    }


    //Window loaded quasi
    public void OnGet(string errorText)
    {
        ErrorText = errorText;
        // int count = _db.UserNames.Count();
        // ErrorText = count.ToString();
    }

    public IActionResult OnPostLogin(LoginDto body)
    {
        if (body.Username == Username && body.Password == Password)
        {
            return new RedirectToPageResult("MainWindow");
        }
        else
        {
            // return new RedirectToPageResult("Index", new { ErrorText = "Username or Password is wrong" });
            return new RedirectToPageResult("MainWindow");
        }

        if (_db.UserNames.Where(x => x.Username == body.Username && x.PasswordHash == body.Password).Count() == 1)
        {
            return new RedirectToPageResult("MainWindow");
        }
    }
}