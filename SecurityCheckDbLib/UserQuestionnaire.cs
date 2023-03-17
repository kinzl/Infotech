using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class UserQuestionnaire
{
    public int UserQuestionnaireId { get; set; }

    public int? UserId { get; set; }

    public int? QuestionnaireId { get; set; }

    public virtual Questionnaire? Questionnaire { get; set; }

    public virtual UserName? User { get; set; }
}
