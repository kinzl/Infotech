using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class SurveyQuestion
{
    public int SurveyQuestionId { get; set; }

    public int? QuestionId { get; set; }

    public int? CustomerSurveyId { get; set; }
    public int Version { get; set; }
    public DateTime CreatedDate { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Questionnaire? Questionnaire { get; set; }
    public virtual UserName? UserName { get; set; }
    public List<CustomerSurvey> CustomerSurvey { get; set; }

}
