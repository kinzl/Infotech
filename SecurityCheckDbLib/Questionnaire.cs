using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Questionnaire
{
    public int QuestionnaireId { get; set; }

    public string QuestionnaireName { get; set; } = null!;

    public virtual ICollection<CustomerSurvey> CustomerSurveys { get; } = new List<CustomerSurvey>();

    public virtual ICollection<UserQuestionnaire> UserQuestionnaires { get; } = new List<UserQuestionnaire>();
}
