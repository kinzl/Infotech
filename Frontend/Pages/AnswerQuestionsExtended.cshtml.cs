using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class AnswerQuestionsExtendedModel : PageModel
{
    public string? CompanyName { get; set; } = "AnyCompany";
    public string? SecurityCheckType { get; set; } = "Security Check Light";

    public void OnGet()
    {
    }

    public void OnPostDownloadSecurityCheck()
    {
        Console.WriteLine("Download");
    }

    public void OnPostSaveSecurityCheck()
    {
        
    }
}