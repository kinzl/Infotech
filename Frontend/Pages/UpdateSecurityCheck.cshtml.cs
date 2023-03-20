using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public List<string> QuestionsList = new() { "Question 1", "Question 2", "Question 3" };
    public List<string> SecurityCheckList = new() { "Security Check Light", "Security Check Extended" };
    public List<string> Criticality = new() { "Middle", "Hard" };
    public List<string> Category = new() { "Category 1", "Category 2" };
    public string SelectedQuestion = "Question 1";
    public int SelectedQuestionIndex = 0;

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

    public void OnPostNewSecurityCheck(string? securityCheckName)
    {
        if (!securityCheckName.IsNullOrEmpty()) SecurityCheckList.Add(securityCheckName);
    }

    public void OnPostQuestionList(string selectedItem)
    {
        SelectedQuestion = selectedItem;
        for (int i = 0; i < QuestionsList.Count; i++)
        {
            if (QuestionsList[i] == selectedItem)
            {
                SelectedQuestionIndex = i;
                break;
            }
        }
    }

    public void OnPostChangeAnswer(string  question, string? answerZero, string? answerOne, string? answerTwo, string? answerThree)
    {
        // if (!answerZero.IsNullOrEmpty() && !answerOne.IsNullOrEmpty() && !answerTwo.IsNullOrEmpty() && !answerThree.IsNullOrEmpty())
        // {
            Console.WriteLine($"question: {question}, \n" +
                              $"0: {answerZero}, \n" +
                              $"1: {answerOne}, \n" +
                              $"2: {answerTwo}, \n" +
                              $"3: {answerThree}");
        // }
        
    }

    public void OnPostDeleteQuestion(string? question)
    {
        Console.WriteLine(question);
    }
}