using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Questionnaire
{
    public int QuestionnaireId { get; set; }

    public string QuestionnaireName { get; set; } = null!;

    public virtual ICollection<SurveyQuestion> SurveyQuestion { get; } = new List<SurveyQuestion>();

}
