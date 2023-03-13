using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AnwerQuestionsModel : PageModel
{

    public List<string> SecurityCheckType = new() { "Security Check Light", "Security Check Extended" };
    
    public void OnGet()
    {
    }

    

    public void OnGetRedirectToAnswerQuestionsExtended()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }

    public void OnPostCompanyName(string companyName)
    {
        Console.WriteLine(companyName);
    }
    
}