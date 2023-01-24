using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace temp.Pages
{
    public class AnwerQuestionsModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnGetUpdateSecurityCheck()
        {
            Response.Redirect("UpdateSecurityCheck");
        }
    }
}
