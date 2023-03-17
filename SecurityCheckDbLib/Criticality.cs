using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Criticality
{
    public int CriticalityId { get; set; }

    public string CriticalityText { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; } = new List<Question>();
}
