using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Questionnaire_Frontend.Pages.Model;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    public string Username = "Admin";
    public string Password = "admin";

    public string InputUsername { get; set; }
    public string InputPassword { get; set; }

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }


    public void OnGet()
    {
    }

    public void OnGetRedirectMainWindow()
    {
        Console.WriteLine("Redirect");
        Response.Redirect("MainWindow");
    }

}