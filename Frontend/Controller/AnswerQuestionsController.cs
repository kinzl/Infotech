namespace Questionnaire_Frontend.Controller;
[Route("[controller]/[action]")]
[ApiController]
public class AnswerQuestionsController
{
    private SecurityCheckContext _db;
    public AnswerQuestionsController(SecurityCheckContext db)
    {
        _db = db;
    }

    [HttpGet("{jsonString}")]
    public List<QuestionDto> DeserializeJsonString(string jsonString)
    {
        Console.WriteLine("Received JSON: \n" + jsonString);
        List<QuestionDto> dataList = JsonSerializer.Deserialize<List<QuestionDto>>(jsonString) ?? new List<QuestionDto>();

        
        
        return dataList;
    }

}