using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

// [ApiController]
// [Route("api/[controller]")]
public class AnswerQuestions : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;
    
    public List<string> SecurityCheckType = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Question = new() { "Question 1", "Question 2","Question 3" };

    public AnswerQuestions(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

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