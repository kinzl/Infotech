using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    private List<string> typSecurityCheck = new List<string>() { "Extended","Light" };
    public void OnGet()
    {
        
    }
}