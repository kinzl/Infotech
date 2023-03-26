using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AsnwerQuestionsModel : PageModel
{
    public List<string> SecurityCheckType = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Question = new() { "Question 1", "Question 2" };

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

    public void OnGetSave()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }
}