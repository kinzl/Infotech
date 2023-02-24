using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AnwerQuestionsModel : PageModel
{

    public void OnGet()
    {
    }

    

    public void OnGetRedirectToAnswerQuestionsExtended()
    {
        Response.Redirect("AnswerQuestionsExtended");
    }
    
}