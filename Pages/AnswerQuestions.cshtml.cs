using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AnwerQuestionsModel : PageModel
{
    private List<int> rbList = new List<int>()
    {
        1, 2, 3, 4
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