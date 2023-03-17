using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class CriticismType
{
    public int CriticismTypeId { get; set; }

    public string CriticismTypeText { get; set; } = null!;

    public virtual ICollection<Criticism> Criticisms { get; } = new List<Criticism>();
}
