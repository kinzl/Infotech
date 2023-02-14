using System.Data;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    
    public IndexModel()
    {
        
    }

    public void OnGet()
    {
    }

    public void OnGetRedirectMainWindow()
    {
        Response.Redirect("MainWindow");
    }
}
