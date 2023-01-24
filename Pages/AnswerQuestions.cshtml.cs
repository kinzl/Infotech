using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace temp.Pages
{
    public class AnwerQuestionsModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnGetRedirectToUpdateSecurityCheck()
        {
            Response.Redirect("UpdateSecurityCheck");
        }

        public void OnGetRedirectToAnwerQuestionsExtended()
        {
            Response.Redirect("AnswerQuestionsExtended");
        }
    }
}
