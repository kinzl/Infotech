using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

// [ApiController]
// [Route("api/[controller]")]
public class AnswerQuestions : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;


    public List<string> SecurityCheckType = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Question = new() { "Question 1", "Question 2", "Question 3" };

    public int SelectedSecurityCheck;

    public AnswerQuestions(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet()
    {
        Initialize();
    }

    private void Initialize()
    {
        SecurityCheckType = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        SelectedSecurityCheck = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheck" ?? "0"));
        
        // var thisSurvey = _db.CustomerSurveys
        //     .Include(x => x.SurveyQuestion)
        //     .Where(x => x.SurveyQuestion.Questionnaire != null && x.SurveyQuestion.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
        //     .Select(x => x.SurveyQuestion)
        //     .LastOrDefault();
        
        Question = _db.SurveyQuestions
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => x.Question.QuestionText)
            .ToList();

    }

    public JsonResult OnPostMyCSharpMethod(string input)
    {
        return new JsonResult("Hello, " + input + "!");
    }
    
    public IActionResult OnPostSecurityCheckTypeChanged(string securityCheck)
    {
        Initialize();
        for (int i = 0; i < SecurityCheckType.Count; i++)
        {
            if (SecurityCheckType[i] == securityCheck)
            {
                HttpContext.Session.SetString("SelectedSecurityCheck", i.ToString());
                break;
            }
        }
        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnGetRedirectToAnswerQuestionsExtended()
    {
        return new RedirectToPageResult("AnswerQuestionsExtended");
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