namespace Questionnaire_Frontend.Dto;

public class AnswerQuestionDto
{
    public string Question { get; set; }
    public string Criticality { get; set; }
    public string Category { get; set; }
    public AnswerDto Answer { get; set; }
    public string Reason { get; set; }
    
}