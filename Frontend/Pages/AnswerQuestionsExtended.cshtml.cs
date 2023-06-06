using CreatePDFReport;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class AnswerQuestionsExtendedModel : PageModel
{
    private readonly ILogger<AnswerQuestionsExtendedModel> _logger;
    private SecurityCheckContext _db;

    public AnswerQuestionsExtendedModel(ILogger<AnswerQuestionsExtendedModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public List<QuestionDto> AllQuestionsAndAnswers = new();

    public string? CompanyName { get; set; } 
    public string? SecurityCheckType { get; set; } 

    public IActionResult OnGet()
    {
        if (HttpContext.User.Identities.ToList().First().Name == null) return new BadRequestResult();
        try
        {
            Initialize();
        }
        catch (Exception e)
        {
            return new RedirectToPageResult("MainWindow", new { ErrorText = "The first Security Check might have no questions or there is no security check" });
        }
        return null;
    }

    private void Initialize()
    {
        _logger.LogInformation("AnswerQuestionsExtendedModel Initialize");
        // SelectedSecurityCheckIndex + 1 = primaryKey
        int index = Convert.ToInt32(HttpContext.Session.GetString("SelectedSecurityCheckIndexMainWindow") ?? "0") + 1;
        var reasonType = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Reason").Single();
        var recommendationType = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Recommendation").Single();
        var riskType = _db.CriticismTypes.Where(x => x.CriticismTypeText == "Risk").Single();
        var risk = _db.SurveyQuestions.Include(x => x.CustomerSurvey).Include(x => x.Question).Include(x => x.Question.Criticisms)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => x.Question.Criticisms.Where(x => x.CriticismType == riskType))
            .ToList();

        AllQuestionsAndAnswers = _db.SurveyQuestions
            .Include(x => x.CustomerSurvey)
            .Include(x => x.Question)
            .Include(x => x.Questionnaire)
            .Include(x => x.Question.Criticality)
            .Include(x => x.Question.Answers)
            .Include(x => x.Question.Category)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => new QuestionDto()
            {
                CompanyName = x.CustomerSurvey.CompanyName,
                Category = x.Question.Category.CategoryText,
                Question = x.Question.QuestionText,
                Criticality = x.Question.Criticality.CriticalityText,
                Questionnaire = x.Questionnaire.QuestionnaireName,
                CustomerSurveyId = index,
                Recommendation = x.Question.Criticisms.Where(x => x.CriticismType == recommendationType).Select(x => x.CriticismText).Single(),
                Reason = x.Question.Criticisms.Where(x => x.CriticismType == reasonType).Select(x => x.CriticismText).Single(),
                Risk = x.Question.Criticisms.Where(x => x.CriticismType == riskType).Select(x => x.CriticismText).Single(),
                Answer = new AnswerDto()
                {
                    AnswerZero = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 0).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 0).Select(x => x.IsChecked).Single(),
                        Points = 0
                    },
                    AnswerOne = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 1).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 1).Select(x => x.IsChecked).Single(),
                        Points = 1
                    },
                    AnswerTwo = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 2).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 2).Select(x => x.IsChecked).Single(),
                        Points = 2
                    },
                    AnswerThree = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == 3).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == 3).Select(x => x.IsChecked).Single(),
                        Points = 3
                    },
                    NotAnswered = new DetailAnswerDto()
                    {
                        Answertext = x.Question.Answers.Where(x => x.Points == -1).Select(x => x.AnswerText).Single(),
                        Selected = x.Question.Answers.Where(x => x.Points == -1).Select(x => x.IsChecked).Single(),
                        Points = -1
                    }
                }
            })
            .ToList();
        CompanyName = _db.SurveyQuestions
            .Include(x => x.CustomerSurvey)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => x.CustomerSurvey.CompanyName)
            .First() ?? "";
        SecurityCheckType = _db.SurveyQuestions
            .Include(x => x.Questionnaire)
            .Where(x => x.CustomerSurveyId == index)
            .Select(x => x.Questionnaire.QuestionnaireName)
            .First();
    }

    public IActionResult OnPostDownloadSecurityCheck()
    {
        _logger.LogInformation("AnswerQuestionsExtendedModel OnPostDownloadSecurityCheck");
        PDFReport pdf = new PDFReport(_db, AllQuestionsAndAnswers);
        pdf.CreatePDF();
        return new RedirectToPageResult("MainWindow");
    }

    public IActionResult OnPostRedirectMainWindow()
    {
        _logger.LogInformation("AnswerQuestionsExtendedModel OnPostRedirectMainWindow");
        return new RedirectToPageResult("MainWindow");
    }
}