using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class AnswerQuestionsExtendedModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public AnswerQuestionsExtendedModel(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

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