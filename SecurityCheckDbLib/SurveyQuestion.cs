﻿using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class SurveyQuestion
{
    public int SurveyQuestionId { get; set; }

    public int? QuestionId { get; set; }

    public int? CustomerSurveyId { get; set; }

    public virtual CustomerSurvey? CustomerSurvey { get; set; }

    public virtual Question? Question { get; set; }
}