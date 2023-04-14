using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

// [ApiController]
// [Route("api/[controller]")]
public class AnswerQuestions : PageModel
{
    public List<string> SecurityCheckType = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Question = new() { "Question 1", "Question 2","Question 3" };

    public void OnGet()
    {
    }
    public JsonResult OnPostMyCSharpMethod(string input)
    {
        return new JsonResult("Hello, " + input + "!");
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