using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Answer
{
    public int AnswerId { get; set; }

    public string? AnswerText { get; set; }

    public int? Points { get; set; }

    public int? QuestionId { get; set; }

    public bool IsChecked { get; set; }

    public virtual Question? Question { get; set; }
}
