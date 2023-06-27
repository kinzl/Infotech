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
    private readonly ILogger<UpdateSecurityCheck> _logger;
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

    public UpdateSecurityCheck(ILogger<UpdateSecurityCheck> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult OnGet(string errorText)
    {
        _logger.LogInformation("UpdateSecurityCheck OnGet");
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        ErrorText = errorText;
        Initialize();
        return null;
    }

    private IActionResult Initialize()
    {
        _logger.Log(LogLevel.Information,"UpdateSecurityCheck Initialize");
        //Initzialie Indexes
        SelectedQuestionPoolIndex = int.Parse(HttpContext.Session.GetString("SelectedQuestionPoolIndex") ?? "0");
        SelectedCategoryIndex = int.Parse(HttpContext.Session.GetString("SelectedCategoryIndex") ?? "0");
        SelectedSecurityCheckQuestionPoolIndex =
            int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckQuestionPoolIndex") ?? "0");
        SelectedSecurityCheckIndex = int.Parse(HttpContext.Session.GetString("SelectedSecurityCheckIndex") ?? "0");

        //Initialize List
        CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
        SecurityCheckList = _db.Questionnaires.Select(x => x.QuestionnaireName).ToList();
        QuestionsListPool = !_db.SurveyQuestions.Where(x => x.CustomerSurveyId == null).Where(x => x.Questionnaire.QuestionnaireId == null).Select(x => x).ToList().IsNullOrEmpty()
            ? _db.SurveyQuestions.Where(x => x.CustomerSurveyId == null).Where(x => x.Questionnaire.QuestionnaireId == null).Select(x => x.Question.QuestionText).ToList()!
            : new List<string>() { "" };

        SecurityCheckQuestionListPool = _db.SurveyQuestions
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
            .Where(x => x.CustomerSurveyId == null)
            .Select(x => x.Question.QuestionText)
            .ToList()
            .IsNullOrEmpty()
            ? new List<string>()
            : _db.SurveyQuestions
                .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
                .Where(x => x.CustomerSurveyId == null)
                .Select(x => x.Question.QuestionText)
                .ToList()!;
        
            var question = _db.SurveyQuestions
                .Include(x => x.CustomerSurvey)
                .Include(x => x.Question)
                .Include(x => x.Question.Answers)
                .Include(x => x.Question.Category)
                .Where(x => x.CustomerSurveyId == null)
                .Where(x => x.Questionnaire.QuestionnaireId == null)
                .Where(x => x.Question.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex])
                .Select(x => x.Question)
                .SingleOrDefault();


            if (question != null)
            {
                txtAnswerZero = question.Answers.SingleOrDefault(x => x.Points == 0)!.AnswerText ?? "";
                txtAnswerOne = question.Answers.SingleOrDefault(x => x.Points == 1)!.AnswerText ?? "";
                txtAnswerTwo = question.Answers.SingleOrDefault(x => x.Points == 2)!.AnswerText ?? "";
                txtAnswerThree = question.Answers.SingleOrDefault(x => x.Points == 3)!.AnswerText ?? "";

                var selectedCategoryIndex = _db.Categories.Where(x => x.CategoryText == question.Category.CategoryText).Single();
                for (int i = 0; i < CategoryList.Count; i++)
                {
                    if (CategoryList[i] == selectedCategoryIndex.CategoryText)
                    {
                        SelectedCategoryIndex = i;
                        break;
                    }
                }
            }
        //}
        //catch (Exception e)
        //{
        //    return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "New Question already exists" });
        //}
        return null;
    }

    public IActionResult OnGetRedirectToMainWindow()
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnGetRedirectToMainWindow");
        return new RedirectToPageResult("MainWindow");
    }

    #region SecurityCheckType

    public IActionResult OnPostNewSecurityCheck(string? securityCheckName)
    {
        if (_db.Questionnaires.Where(x => x.QuestionnaireName == securityCheckName).Select(x => x.QuestionnaireName).ToList().Count() > 0)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Security Check exists" });
        }
        if (!securityCheckName.IsNullOrEmpty())
        {
            _db.Questionnaires.Add(new Questionnaire() { QuestionnaireName = securityCheckName });
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        else
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Enter a security check name" });
        }

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
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostSecurityCheckQuestionListChanged");
        Initialize();
        HttpContext.Session.SetString("SelectedSecurityCheckQuestionPoolIndex",
            IndexOfItemInList(SecurityCheckQuestionListPool, selectedQuestion));
        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostAddQuestionToSecurityCheck()
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostAddQuestionToSecurityCheck");
        Initialize();
        try
        {
            var question = _db.SurveyQuestions
                .Include(x => x.CustomerSurvey)
                .Include(x => x.Question)
                .Include(x => x.Question.Answers)
                .Where(x => x.CustomerSurveyId == null)
                .Where(x => x.Questionnaire.QuestionnaireId == null)
                .Where(x => x.Question.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex])
                .Select(x => x.Question)
                .Single();

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
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "No question or security check found" });
        }
    }

    public IActionResult OnPostDeleteQuestionFromSecurityCheck()
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostDeleteQuestionFromSecurityCheck");
        Initialize();
        try
        {
            var question = _db.SurveyQuestions
            .Where(x => x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex])
            .Where(x => x.Question.QuestionText == SecurityCheckQuestionListPool[SelectedSecurityCheckQuestionPoolIndex])
            .Where(x => x.Questionnaire.QuestionnaireId != null)
            .Where(x => x.CustomerSurveyId == null)
            .Select(x => x.Question)
            .First();

            var surveyObject = _db.SurveyQuestions.FirstOrDefault(x =>
                x.Questionnaire != null && x.Question == question &&
                x.Questionnaire.QuestionnaireName == SecurityCheckList[SelectedSecurityCheckIndex]);

            if (surveyObject != null) _db.SurveyQuestions.Remove(surveyObject);
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "No Question to remove found" });
        }
    }

    public IActionResult OnPostQuestionListPoolChanged(string selectedQuestion)
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostQuestionListPoolChanged");
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
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostCategoryChanged");
        Initialize();

        var newCategoryIndex = IndexOfItemInList(CategoryList, categoryItem);
        SelectedCategoryIndex = int.Parse(newCategoryIndex);
        var newCategory = _db.Categories.Where(x => x.CategoryText == CategoryList[SelectedCategoryIndex]).Single();
        var question = _db.SurveyQuestions
            .Include(x => x.Question.Category)
            .Where(x => x.Question.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex])
            .Where(x => x.Questionnaire.QuestionnaireId == null)
            .Where(x => x.CustomerSurveyId == null)
            .Select(x => x.Question)
            .Single();

        question.Category = newCategory;
        _db.SaveChanges();
        HttpContext.Session.SetString("SelectedCategoryIndex", newCategoryIndex);
        

        return new RedirectToPageResult("UpdateSecurityCheck");
    }

    public IActionResult OnPostAddNewCategory(string newCategory)
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostAddNewCategory");
        Initialize();
        if (_db.Categories.Where(x => x.CategoryText == newCategory).ToList().Count() > 0)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Category exists" });
        }
        try
        {
            _db.Categories.Add(new Category() { CategoryText = newCategory });
            _db.SaveChanges();
            CategoryList = _db.Categories.Select(x => x.CategoryText).ToList();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "New Category can not be empty" });
        }
    }

    public IActionResult OnPostDeleteSelectedCategory(string categoryItem)
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostDeleteSelectedCategory");
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
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "No Category found" });
        }
    }

    #endregion

    private string IndexOfItemInList(List<string> list, string searchString)
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck IndexOfItemInList");
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
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostDeleteQuestion");
        Initialize();
        HttpContext.Session.SetString("SelectedQuestionPoolIndex", "0");
        try
        {
            var removeQuestion = _db.SurveyQuestions
                .Include(x => x.Question)
                .Include(x => x.Question.Answers)
                .Where(x => x.Question.QuestionText == question)
                .Where(x => x.Questionnaire.QuestionnaireId == null)
                .Where(x => x.CustomerSurveyId == null)
                .Select(x => x)
                .First();
            _db.SurveyQuestions.Remove(removeQuestion);
            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");

        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "No Question found" });
        }
    }

    public IActionResult OnPostAddNewQuestion()
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostAddNewQuestion");
        Initialize();
        if (_db.Questions.Where(x => x.QuestionText == "New Question").Select(x => x).ToList().Count > 0)
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Question already exists" });

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
            var newQuestion = new Question()
            {
                QuestionText = "New Question",
                Answers = newAnsweres,
                Category = _db.Categories.SingleOrDefault(
                    x => x.CategoryText == CategoryList[SelectedCategoryIndex]),
            };
            _db.Questions.Add(newQuestion);
            _db.SaveChanges();
            _db.SurveyQuestions.Add(new SurveyQuestion()
            {
                Question = newQuestion
            });

            _db.SaveChanges();
            return new RedirectToPageResult("UpdateSecurityCheck");
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Security Check or Category might be empty" });
        }
    }

    public IActionResult OnPostUpdateQuestion(string question,
        string? answerZero,
        string? answerOne,
        string? answerTwo,
        string? answerThree)
    {
        _logger.Log(LogLevel.Information, "UpdateSecurityCheck OnPostUpdateQuestion");
        Initialize();

        HttpContext.Session.SetString("SelectedQuestionPoolIndex","0");
        if (answerZero.IsNullOrEmpty() || answerOne.IsNullOrEmpty() || answerTwo.IsNullOrEmpty() ||
            answerThree.IsNullOrEmpty())
            return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Any Answer might be empty" });
        
        //if (_db.SurveyQuestions
        //    .Where(x => x.CustomerSurveyId == null)
        //    .Where(x => x.Questionnaire.QuestionnaireId == null)
        //    .Where(x => x.Question.QuestionText == question)
        //    .ToList().Count > 1)
        //    return new RedirectToPageResult("UpdateSecurityCheck", new { ErrorText = "Question already exists" });

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

        var selectedQuestion = _db.SurveyQuestions
            .Include(x => x.Question)
            .Include(x => x.Question.Answers)
            .Where(x => x.CustomerSurveyId == null)
            .Where(x => x.Questionnaire.QuestionnaireId == null)
            .Where(x => x.Question.QuestionText == QuestionsListPool[SelectedQuestionPoolIndex])
            .FirstOrDefault();

        selectedQuestion.Question.QuestionText = question;
        selectedQuestion.Question.Answers = newAnsweres;
        selectedQuestion.Question.Category = _db.Categories.Where(x => x.CategoryText == CategoryList[SelectedCategoryIndex])
            .SingleOrDefault();

        _db.SaveChanges();

        return new RedirectToPageResult("UpdateSecurityCheck");
    }
}