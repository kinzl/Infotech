﻿using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Question
{
    public int QuestionId { get; set; }

    public int? CategoryId { get; set; }

    public int? CriticalityId { get; set; }

    public string? QuestionText { get; set; }

    public virtual ICollection<Answer> Answers { get; } = new List<Answer>();

    public virtual Category? Category { get; set; }

    public virtual Criticality? Criticality { get; set; }

    public virtual ICollection<Criticism> Criticisms { get; } = new List<Criticism>();

    public virtual ICollection<SurveyQuestion> SurveyQuestions { get; } = new List<SurveyQuestion>();
}