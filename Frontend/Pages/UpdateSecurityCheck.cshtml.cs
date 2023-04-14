using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public List<string> QuestionsList = new()
    {
        "Security Check Light|Werden IT-Systeme überwacht / gibt es ein Monitoring-System, welches die Verfügbarkeit der notwendigen Ressourcen/Systeme überwacht?",
        "Security Check Light|Werden Systeme laufend auch von extern geprüft (Shadowserver, Qualys, Monitoring extern erreichbarer Systeme usw.)?",
        "Security Check Extended|Wird eine Kapazitätsplanung durchgeführt?"
    };

    public List<string> SecurityCheckList = new()
    {
        "Security Check Light",
        "Security Check Extended"
    };

    public List<string> Category = new()
    {
        "1 Organisation",
        "2 Nutzungsrichtlinie",
        "3 Geheimhaltung und Datenschutz",
        "4 Asset- und Risikomanagement",
        "5 Notfallmanagement",
        "6 Awareness",
        "7 Systembetrieb",
        "8 Netzwerk und Kommunikation",
        "9 Zutritts- und Zugriffsberechtigungen"
    };

    public string SelectedSecurityCheck;
    public static string SelectedQuestion;
    public string SecurityCheckItemToShow;
    public int SelectedQuestionIndex = 0;

    public void OnGet()
    {
        SelectedQuestion = QuestionsList.First();
        SecurityCheckItemToShow = SecurityCheckList[0];
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

    public void OnPostQuestionList(string selectedQuestion)
    {
        SelectedQuestion = selectedQuestion;
        for (int i = 0; i < QuestionsList.Count; i++)
        {
            if (QuestionsList[i] == selectedQuestion)
            {
                SelectedQuestionIndex = i;
                int index = SecurityCheckList.IndexOf(QuestionsList[i].Split("|")[0]);
                var item = SecurityCheckList[index];
                SecurityCheckList.RemoveAt(index);
                SecurityCheckList.Add(item);
                break;
            }
        }
    }

    public void OnPostSecurityCheckChanged(string selectedItem)
    {
        var splitted = SelectedQuestion.Split("|");
        for (int i = 0; i < QuestionsList.Count; i++)
        {
            if (QuestionsList[i] == SelectedQuestion)
            {
                QuestionsList[i] = $"{selectedItem}|{splitted[1]}";
                break;
            }
        }
    }

    public void OnPostChangeAnswer(string question, string? answerZero, string? answerOne, string? answerTwo,
        string? answerThree)
    {
        // if (!answerZero.IsNullOrEmpty() && !answerOne.IsNullOrEmpty() && !answerTwo.IsNullOrEmpty() && !answerThree.IsNullOrEmpty())
        // {
        //if question already exists return;
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
        int index = 0;
        for (int i = 0; i < QuestionsList.Count; i++)
        {
            if (QuestionsList[i] == question)
            {
                index = i;
                QuestionsList.RemoveAt(index);
                break;
            }
        }
    }
}