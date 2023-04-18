using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecurityCheckDbLib;

public partial class UserName
{
    [Key]
    public int UserNameId { get; set; }

    public string Username { get; set; } = null!;

    public bool IsAdmin { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<UserQuestionnaire> UserQuestionnaires { get; } = new List<UserQuestionnaire>();
}
