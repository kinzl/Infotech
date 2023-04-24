using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class UpdateSecurityCheck : PageModel
{
    public string? ErrorText;
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public List<string> QuestionsListPool = new()
    {
        // "Werden IT-Systeme überwacht / gibt es ein Monitoring-System, welches die Verfügbarkeit der notwendigen Ressourcen/Systeme überwacht?",
        // "Werden Systeme laufend auch von extern geprüft (Shadowserver, Qualys, Monitoring extern erreichbarer Systeme usw.)?",
        // "Wird eine Kapazitätsplanung durchgeführt?"
    };

    public List<string> SecurityCheckQuestionListPool = new()
    {
        // "Werden IT-Systeme überwacht / gibt es ein Monitoring-System, welches die Verfügbarkeit der notwendigen Ressourcen/Systeme überwacht?",
        // "Werden Systeme laufend auch von extern geprüft (Shadowserver, Qualys, Monitoring extern erreichbarer Systeme usw.)?",
        // "Wird eine Kapazitätsplanung durchgeführt?"
    };

    public List<string> SecurityCheckList = new()
    {
        // "Security Check Light",
        // "Security Check Extended"
    };

    public List<string> CategoryList = new()
    {
        // "1 Organisation",
        // "2 Nutzungsrichtlinie",
        // "3 Geheimhaltung und Datenschutz",
        // "4 Asset- und Risikomanagement",
        // "5 Notfallmanagement",
        // "6 Awareness",
        // "7 Systembetrieb",
        // "8 Netzwerk und Kommunikation",
        // "9 Zutritts- und Zugriffsberechtigungen"
    };

    public int SelectedQuestionPoolIndex;
    public int SelectedSecurityCheckQuestionPoolIndex;
    public int SelectedCategoryIndex;
    public int SelectedSecurityCheckIndex;

    //int index = HttpContext.Session.GetString("ProductsSortType") ?? DefaultSortType;
    //HttpContext.Session.SetString("OrdersSortType", OrdersSortType);

    public UpdateSecurityCheck(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet(string errorText)
    {
        ErrorText = errorText;
        Initialize();
    }

    private void Initialize()
    {
        //Initzialie Indexes
        SelectedQuestionPoolIndex = int.Parse(HttpContext.Session.GetString("SelectedQuestionPoolIndex") ?? "0");
        SelectedCategoryIndex = int.Parse(HttpContext.Session.GetString("SelectedCategoryIndex") ?? "0");
        SelectedSecurityCheckQuestionPoolIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckQuestionPoolIndex") ?? "0");
        SelectedSecurityCheckIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0");

        //Initialize List
        CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
        SecurityCheckList = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        QuestionsListPool = !_db.Questions.Select(x => x.QuestionText).ToList().IsNullOrEmpty()
            ? _db.Questions.Select(x => x.QuestionText).ToList()!
            : new List<string>() { "" };
        SecurityCheckQuestionListPool = _db.SurveyQuestions
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
            .Select(x => x.Question.QuestionText)
            .ToList();

        
    }

    public IActionResult OnGetRedirectToMainWindow()
    {
        return new RedirectToPageResult("MainWindow");
    }

    #region SecurityCheckType

    public IActionResult OnPostNewSecurityCheck(string? securityCheckName)
    {
        if (!securityCheckName.IsNullOrEmpty())
        {
            _db.Questionnaires.Add(new Questionnaire() { QuestionnaireName = securityCheckName });
            _db.SaveChanges();
        }

        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostSecurityCheckQuestionPoolChanged(string securityCheckItem)
    {
        Initialize();

        SecurityCheckQuestionListPool = _db.SurveyQuestions
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
            .Select(x => x.Question.QuestionText)
            .ToList();
        
        SelectedSecurityCheckIndex = SecurityCheckList.IndexOf(securityCheckItem);
        HttpContext.Session.SetString("SelectedSecurityCheckIndex", SelectedSecurityCheckIndex.ToString());

        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    #endregion

    public IActionResult OnPostSecurityCheckQuestionListChanged(string selectedQuestion)
    {
        Initialize();
        HttpContext.Session.SetString("SelectedSecurityCheckQuestionPoolIndex", IndexOfItemInList(SecurityCheckQuestionListPool, selectedQuestion));

        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostAddQuestionToSecurityCheck()
    {
        Initialize();
        Question question = _db.Questions
            .Single(x => x.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex]);

        string categoryString = CategoryList[SelectedCategoryIndex];
        var category = _db.Categories.SingleOrDefault(x => x.CategoryText == categoryString);
        question.Category = category;
        var questionnaire = _db.Questionnaires.SingleOrDefault(x => x.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex]);

        _db.SurveyQuestions.Add(new SurveyQuestion()
        {
            Question = question,
            Questionnaire = questionnaire,
        });
        _db.SaveChanges();
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostDeleteQuestionFromSecurityCheck()
    {
        Initialize();
        Question question = _db.Questions.Where(x =>
            x.QuestionText == SecurityCheckQuestionListPool[SelectedSecurityCheckQuestionPoolIndex]).Single();

        var surveyObject = _db.SurveyQuestions.FirstOrDefault(x => x.Questionnaire != null && x.Question == question && x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex]);
        
        if(surveyObject != null)_db.SurveyQuestions.Remove(surveyObject);
        _db.SaveChanges();
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostQuestionListPoolChanged(string selectedQuestion)
    {
        Initialize();
        for (int i = 0; i < QuestionsListPool.Count; i++)
        {
            if (QuestionsListPool[i] == selectedQuestion)
            {
                HttpContext.Session.SetString("SelectedQuestionPoolIndex", i.ToString());
                break;
            }
        }

        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    #region ############Criticality

    public IActionResult OnPostCategoryChanged(string categoryItem)
    {
        Initialize();

        HttpContext.Session.SetString("SelectedCategoryIndex", IndexOfItemInList(CategoryList, categoryItem));
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostAddNewCategory(string newCategory)
    {
        Initialize();
        _db.Categories.Add(new Category() { CategoryText = newCategory });
        _db.SaveChanges();
        CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostDeleteSelectedCategory(string categoryItem)
    {
        Initialize();
        var item = _db.Categories.Where(x => x.CategoryText == categoryItem).SingleOrDefault();
        _db.Categories.Remove(item);
        _db.SaveChanges();
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    #endregion

    private string IndexOfItemInList(List<string> list, string searchString)
    {
        string index = "0";

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == searchString)
            {
                index = i.ToString();
                break;
            }
        }

        return index;
    }


    public void OnPostChangeAnswer(string question, string? answerZero, string? answerOne, string? answerTwo,
        string? answerThree)
    {
        if (!answerZero.IsNullOrEmpty() && !answerOne.IsNullOrEmpty() && !answerTwo.IsNullOrEmpty() &&
            !answerThree.IsNullOrEmpty())
        {
            //if question already exists return;
            Console.WriteLine($"question: {question}, \n" +
                              $"0: {answerZero}, \n" +
                              $"1: {answerOne}, \n" +
                              $"2: {answerTwo}, \n" +
                              $"3: {answerThree}");
        }
    }

    public void OnPostDeleteQuestion(string? question)
    {
        Console.WriteLine(question);
        int index = 0;
        for (int i = 0; i < QuestionsListPool.Count; i++)
        {
            if (QuestionsListPool[i] == question)
            {
                index = i;
                QuestionsListPool.RemoveAt(index);
                break;
            }
        }
    }

    public IActionResult OnPostAddNewQuestion()
    {
        Initialize();
        _db.Questions.Add(new Question()
        {
            QuestionText = "New Question",
            Category = _db.Categories.Where(x => x.CategoryText == CategoryList[SelectedCategoryIndex]).SingleOrDefault(),
            Answers = new List<Answer>()
        });
        _db.SaveChanges();
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostUpdateQuestion(string question,
        string? answerZero,
        string? answerOne,
        string? answerTwo,
        string? answerThree)
    {
        Initialize();
        List<Answer> newAnsweres = new()
        {
            new Answer()
            {
                AnswerText = answerOne,
                Question = _db.Questions.Where(x => x.QuestionText == question).FirstOrDefault()
            }
        };
        var selectedQuestion = _db.Questions.Where(x => x.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex])
            .FirstOrDefault();
        selectedQuestion.QuestionText = question;
        selectedQuestion.Answers = newAnsweres;
        selectedQuestion.Category = _db.Categories.Where(x => x.CategoryText == CategoryList[SelectedCategoryIndex])
            .SingleOrDefault();

        _db.SaveChanges();

        return new RedirectToPageResult("UpdateSecurityCheck");
    }
}