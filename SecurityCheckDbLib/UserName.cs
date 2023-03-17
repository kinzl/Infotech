using System;
using System.Collections.Generic;

namespace SecurityCheckDbLib;

public partial class UserName
{
    public int UserId { get; set; }

    public string Username1 { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<UserQuestionnaire> UserQuestionnaires { get; } = new List<UserQuestionnaire>();
}
