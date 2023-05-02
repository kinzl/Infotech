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
            // SeederExtension.Seed(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    public void OnGet(string errorText)
    {
        ErrorText = errorText;
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

        //ToDo: Convert password to salt and hash and compare them
        if (_db.UserNames.Where(x => x.Username == body.Username && x.PasswordHash == body.Password).Count() == 1)
        {
            return new RedirectToPageResult("MainWindow");
        }
    }
}