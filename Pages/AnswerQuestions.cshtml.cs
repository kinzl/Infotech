using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AnwerQuestionsModel : PageModel
{
    private List<string> rbList = new()
    {
        "n.A.", "1", "2", "3"
    };

    public void OnGet()
    {
    }

    public void OnGetRedirectToUpdateSecurityCheck()
    {
        Response.Redirect("UpdateSecurityCheck");
    }

    public void OnGetRedirectToAnswerQuestionsExtended()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }
    
}