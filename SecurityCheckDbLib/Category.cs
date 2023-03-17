using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryText { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; } = new List<Question>();
}
