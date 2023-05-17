namespace Questionnaire_Frontend.Dto;

public class QuestionDto
{
    public string Question { get; set; }
    public string Category { get; set; }
    public AnswerDto Answer { get; set; }
    public string Criticality { get; set; }

    public string Reason { get; set; }
    public string Risk { get; set; }
    public string Recommendation { get; set; }
    public string Questionnaire { get; set; }
    public string CompanyName { get; set; }
}