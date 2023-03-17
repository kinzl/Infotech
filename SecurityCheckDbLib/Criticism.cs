using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Criticism
{
    public int CriticismId { get; set; }

    public string? CriticismText { get; set; }

    public int? CriticismTypeId { get; set; }

    public int? QuestionId { get; set; }

    public virtual CriticismType? CriticismType { get; set; }

    public virtual Question? Question { get; set; }
}
