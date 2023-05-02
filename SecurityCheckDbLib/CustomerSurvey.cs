using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCheckDbLib;
public class CustomerSurvey
{
    public int CustomerSurveyId { get; set; }
    public SurveyQuestion SurveyQuestion { get; set; }
    public string? CompanyName { get; set; }
    public string ToDataString()
    {
        return
            $"{CompanyName} | {SurveyQuestion.CreatedDate.ToString("yy-MM-dd HH:mm:ss")} | {SurveyQuestion.Questionnaire?.QuestionnaireName}";
    }
}
