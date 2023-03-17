using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class CustomerSurvey
{
    public int CustomerSurveyId { get; set; }

    public string? CompanyName { get; set; }

    public int? QuestionnaireId { get; set; }

    public virtual Questionnaire? Questionnaire { get; set; }

    public virtual ICollection<SurveyQuestion> SurveyQuestions { get; } = new List<SurveyQuestion>();
}
