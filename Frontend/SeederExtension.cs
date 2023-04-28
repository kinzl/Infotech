using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public static class SeederExtension
{
    public static void Seed(SecurityCheckContext db)
    {
        AddUserNames(db);
    }
    
    private static void AddUserNames(SecurityCheckContext db)
    {
        UserName admin = new UserName
        {
            IsAdmin = false,
            Username = "kinzl",
            PasswordHash = "s45g5",
            PasswordSalt = "df45"
        };
        db.UserNames.Add(admin);
        db.SaveChanges();
    }
}