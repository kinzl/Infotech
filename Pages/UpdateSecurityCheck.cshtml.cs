using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public List<string> QuestionsList = new() { "Question 1", "Question 2", "Question 3" };
    public List<string> SecurityCheckList = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Criticality = new() { "Middle", "Hard" };
    public List<string> Category = new() { "Category 1", "Category 2" };

    public void OnGet()
    {
    }

    public void OnGetRedirectToMainWindow()
    {
        Response.Redirect("MainWindow");
    }

    public void OnPostAddNewQuestion()
    {
        QuestionsList.Add("Question xy");
    }
}