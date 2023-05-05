using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Questionnaire_Frontend.Dto;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

// [ApiController]
// [Route("api/[controller]")]
public class AnswerQuestions : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;


    public List<string> SecurityCheckType = new()
    {
        // "Security Check Light",
        // "Security Check Extended"
    };

    public List<string> Question = new()
    {
        // "Question 1",
        // "Question 2", 
        // "Question 3"
    };

    public List<AnswerDto> TestQuestionDto = new ()
    { 
        new AnswerDto()
        {
            AnswerOne = new DetailAnswerDto()
            {
                Answer = "answer one",
                Selected = false
            },
            AnswerTwo = new DetailAnswerDto()
            {
                Answer = "answer two",
                Selected = false
            }
        }
    };

    public int SelectedSecurityCheck;
    public string CompanyName = "";

    public AnswerQuestions(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet()
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        Initialize();
        return null;
    }

    private void Initialize()
    {
        SecurityCheckType = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        SelectedSecurityCheck = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheck") ?? "0");
        CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";

        Question = _db.SurveyQuestions
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => x.Question.QuestionText)
            .ToList();
    }

    public IActionResult OnPostSecurityCheckTypeChanged(string securityCheck)
    {
        Initialize();
        for (int i = 0; i < SecurityCheckType.Count; i++)
        {
            if (SecurityCheckType[i] == securityCheck)
            {
                SelectedSecurityCheck = i;
                HttpContext.Session.SetString("SelectedSecurityCheck", i.ToString());
                break;
            }
        }

        var thisSurvey = _db.CustomerSurveys
            .Include(x => x.SurveyQuestion)
            .ThenInclude(x => x.Questionnaire)
            .Select(x => x)
            .OrderBy(x => x.CustomerSurveyId)
            .Last();
        var survey = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckType[SelectedSecurityCheck])
            .Select(x => x)
            .FirstOrDefault();

        thisSurvey.SurveyQuestion = survey;
        _db.SaveChanges();
        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnGetRedirectToAnswerQuestionsExtended()
    {
        return new RedirectToPageResult("AnswerQuestionsExtended");
    }

    public IActionResult OnPostSubmitCompanyName(string companyName)
    {
        var thisSurvey = _db.CustomerSurveys
            .Select(x => x)
            .OrderBy(x => x.CustomerSurveyId)
            .LastOrDefault();
        thisSurvey.CompanyName = companyName;
        _db.SaveChanges();
        HttpContext.Session.SetString("CompanyName", companyName);
        return new RedirectToPageResult("AnswerQuestions");
    }

    public void OnGetSave()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }
}