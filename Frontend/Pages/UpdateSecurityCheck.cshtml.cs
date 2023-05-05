using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

    public string txtAnswerZero;
    public string txtAnswerOne;
    public string txtAnswerTwo;
    public string txtAnswerThree;
    
    public UpdateSecurityCheck(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet(string errorText)
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        ErrorText = errorText;
        Initialize();
        return null;
    }

    private void Initialize()
    {
        //Initzialie Indexes
        SelectedQuestionPoolIndex = int.Parse(HttpContext.Session.GetString("SelectedQuestionPoolIndex") ?? "0");
        SelectedCategoryIndex = int.Parse(HttpContext.Session.GetString("SelectedCategoryIndex") ?? "0");
        SelectedSecurityCheckQuestionPoolIndex =
            int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckQuestionPoolIndex") ?? "0");
        SelectedSecurityCheckIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0");

        //Initialize List
        CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
        SecurityCheckList = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        QuestionsListPool = !_db.Questions.Select(x => x.QuestionText).ToList().IsNullOrEmpty()
            ? _db.Questions.Select(x => x.QuestionText).ToList()!
            : new List<string>() { "" };

        SecurityCheckQuestionListPool = !_db.SurveyQuestions.Select(x => x.Question).ToList().IsNullOrEmpty()
            ? _db.SurveyQuestions
                .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
                .Select(x => x.Question.QuestionText)
                .ToList()!
            : new List<string>();

        var question =
            _db.Questions
                .Include(x => x.Answers)
                .Include(x => x.Category)
                .SingleOrDefault(x => x.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex]);
        if (question != null)
        {
            txtAnswerZero = question.Answers.SingleOrDefault(x => x.Points == 0)!.AnswerText ?? "";
            txtAnswerOne = question.Answers.SingleOrDefault(x => x.Points == 1)!.AnswerText ?? "";
            txtAnswerTwo = question.Answers.SingleOrDefault(x => x.Points == 2)!.AnswerText ?? "";
            txtAnswerThree = question.Answers.SingleOrDefault(x => x.Points == 3)!.AnswerText ?? "";
        }
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
        HttpContext.Session.SetString("SelectedSecurityCheckQuestionPoolIndex",
            IndexOfItemInList(SecurityCheckQuestionListPool, selectedQuestion));
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostAddQuestionToSecurityCheck()
    {
        Initialize();
        try
        {
            Question question = _db.Questions
                .Single(x => x.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex]);

            string categoryString = CategoryList[SelectedCategoryIndex];
            var category = _db.Categories.SingleOrDefault(x => x.CategoryText == categoryString);
            question.Category = category;
            var questionnaire =
                _db.Questionnaires.SingleOrDefault(
                    x => x.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex]);

            _db.SurveyQuestions.Add(new SurveyQuestion()
            {
                Question = question,
                Questionnaire = questionnaire,
            });
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new {ErrorText = "No question or security check found"});
        }
    }

    public IActionResult OnPostDeleteQuestionFromSecurityCheck()
    {
        Initialize();
        try
        {
            Question question = _db.Questions.Where(x =>
                x.QuestionText == SecurityCheckQuestionListPool[SelectedSecurityCheckQuestionPoolIndex]).Single();

            var surveyObject = _db.SurveyQuestions.FirstOrDefault(x =>
                x.Questionnaire != null && x.Question == question &&
                x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex]);

            if (surveyObject != null) _db.SurveyQuestions.Remove(surveyObject);
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new {ErrorText = "No Question to remove found"});
        }
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
        try
        {
            _db.Categories.Add(new Category() { CategoryText = newCategory });
            _db.SaveChanges();
            CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck",new {ErrorText = "New Category can not be empty"});
        }
    }

    public IActionResult OnPostDeleteSelectedCategory(string categoryItem)
    {
        Initialize();
        try
        {
            var item = _db.Categories.Where(x => x.CategoryText == categoryItem).SingleOrDefault();
            _db.Categories.Remove(item);
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new{ErrorText = "No Category found"});
        }
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

    public IActionResult OnPostDeleteQuestion(string? question)
    {
        Initialize();
        try
        {
            var removeQuestion = _db.Questions.SingleOrDefault(x => x.QuestionText == question);
            _db.Questions.Remove(removeQuestion);
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");

        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck",new {ErrorText = "No Question found"});
        }
    }

    public IActionResult OnPostAddNewQuestion()
    {
        Initialize();
        try
        {
            List<Answer> newAnsweres = new()
            {
                new Answer()
                {
                    AnswerText = "n.A.",
                    Points = -1,
                    IsChecked = true,
                },
                new Answer()
                {
                    AnswerText = "a",
                    Points = 0,
                    IsChecked = false,
                },
                new Answer()
                {
                    AnswerText = "b",
                    Points = 1,
                    IsChecked = false,
                },
                new Answer()
                {
                    AnswerText = "c",
                    Points = 2,
                    IsChecked = false,
                },
                new Answer()
                {
                    AnswerText = "d",
                    Points = 3,
                    IsChecked = false,
                },
            };
            _db.Questions.Add(new Question()
            {
                QuestionText = "New Question",
                Category = _db.Categories
                    .SingleOrDefault(x => x.CategoryText == CategoryList[SelectedCategoryIndex]),
                Answers = newAnsweres
            });
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck",new {ErrorText = "Security Check or Category might be empty"});
        }
    }

    public IActionResult OnPostUpdateQuestion(string question,
        string? answerZero,
        string? answerOne,
        string? answerTwo,
        string? answerThree)
    {
        Initialize();

        if (answerZero.IsNullOrEmpty() || answerOne.IsNullOrEmpty() || answerTwo.IsNullOrEmpty() ||
            answerThree.IsNullOrEmpty())
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Any Answer might be empty" });


        List<Answer> newAnsweres = new()
        {
            new Answer()
            {
                AnswerText = "n.A.",
                Points = -1,
                IsChecked = true,
            },
            new Answer()
            {
                AnswerText = answerZero,
                Points = 0,
                IsChecked = false,
            },
            new Answer()
            {
                AnswerText = answerOne,
                Points = 1,
                IsChecked = false,
            },
            new Answer()
            {
                AnswerText = answerTwo,
                Points = 2,
                IsChecked = false,
            },
            new Answer()
            {
                AnswerText = answerThree,
                Points = 3,
                IsChecked = false,
            },
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