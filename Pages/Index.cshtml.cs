using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Questionnaire_Frontend.Dto;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    public string? ErrorText;
    public string Username = "Admin";
    public string Password = "admin";

    public string InputUsername { get; set; }
    public string InputPassword { get; set; }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }


    //Window loaded quasi
    public void OnGet(string errorText)
    {
        ErrorText = errorText;
    }

    public void OnGetRedirectMainWindow()
    {
        Console.WriteLine("Redirect");
        Response.Redirect("MainWindow");
    }

    public IActionResult OnPostLogin(LoginDto body)
    {
        if (body.Username == Username && body.Password == Password)
        {
            return new RedirectToPageResult("MainWindow");
        }
        else
        {
            return new RedirectToPageResult("Index", new { ErrorText = "Fehler" });
        }
    }
}