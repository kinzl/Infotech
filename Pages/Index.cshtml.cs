using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    public string Username = "Admin";
    public string Password = "admin";

    public IndexModel()
    {
    }

    public void OnGet()
    {
    }

    public void OnGetRedirectMainWindow()
    {
        Response.Redirect("MainWindow");
    }

    public IActionResult OnGetGetAllStudents()
    {
        Console.Write("Among");
        return new OkObjectResult(Username);
    }
    
    public IActionResult OnPostGetMyPostRequest()
    {
        Console.Write("Among: " + "id");
        return new OkObjectResult(Username);
    }
    
}