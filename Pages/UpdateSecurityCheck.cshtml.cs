using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public List<string> list = new() { "Extended","Light", "Extended" };
    public void OnGet()
    {
        
    }
}