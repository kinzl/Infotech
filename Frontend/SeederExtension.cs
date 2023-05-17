using System.Data;

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
            IsAdmin = true,
            Username = "admin",
            PasswordHash = "17A8DFE340516E6417415C2FE29B98E8A7D893DF61035CA26ADAF770EB752F1EB59402565CEC025D12799DDB05714DF0200AAD5B3B5EA9EA2FE5AFCD34209CE2",
            PasswordSalt = "6256A12324AD33225BCA548F4A950B4531808A47CCAC870AB5BE49D624465AA31C9CFC1E02FDF459CB6BD8753A5EF1D1761CFBCC9B3FA46936F206FAF4BBAB36"
        };
        //pw = admin
        db.UserNames.Add(admin);
        UserName kinzle = new UserName
        {
            IsAdmin = false,
            Username = "kinzle",
            PasswordHash = "0F9086BE49A72B9683E37809994056CA66CE56E9C7F17E3BDD4D38BF684B1A741D4080E1A8B40AFC2763C9408A71D1B2FD3DE110D47C603E7C8CD0AABF0F8B2A",
            PasswordSalt = "A8A7304F32C2EAA6DF6169D05FF348822B1BEA162640C5F4DAEDC6162F642754325FC3E44099DDF348179C7C9010AADD348685AA07161E6F2B212AB18FCB645C"
        };
        //pw = binProjektLeiter*3
        db.UserNames.Add(kinzle);
        UserName davk = new UserName
        {
            IsAdmin = false,
            Username = "davk",
            PasswordHash = "9316F2B4B3EE2AFE5D429C907B7A08C464AC9D5381CB9C794CD4D7448D989341A8D7A285BA21A8564115E5E9C4AC39047F982AE118E27970F6F8F45FACC1B851",
            PasswordSalt = "F6D1A4CF1CA463920D8D9534B414B9C19263B64019493A1CFE1E4E94780B9CD5ECE2A4815A8E87087F654368F4B6BE3041ABA48E5762CDE56D9BC05373EF17A9"
        };
        //pw = IchbinDavK#1
        db.UserNames.Add(davk);
        UserName ich = new UserName
        {
            IsAdmin = false,
            Username = "ich",
            PasswordHash = "60434C1A11F09A04994A2E8F6AE05E23444E6352BA49E7807A0CB9A35A0315AD0D82DF231B9645A9E51AACE2E8294CAB33580377C60B5AF11A9E06B717B0A735",
            PasswordSalt = "0622302726E4D795429F71E962D15A95DC75D5F353E9F0ABC0D09CE32E0A5469D29F01D3EF50D9EA080917F87626BA5A278E05402C1FAC98C86DC5BB8B853A87"
        };
        //pw = ich
        db.UserNames.Add(ich);
        Criticality cr1 = new Criticality()
        {
            CriticalityText = "Hoch",
        };
        db.Criticalities.Add(cr1);
        Criticality cr2 = new Criticality()
        {
            CriticalityText = "Mittel",
        };
        db.Criticalities.Add(cr2);
        Criticality cr3 = new Criticality()
        {
            CriticalityText = "Niedrig",
        };
        db.Criticalities.Add(cr3);
        CriticismType ct1 = new CriticismType()
        {
            CriticismTypeText = "Reason"
        };
        db.CriticismTypes.Add(ct1);
        CriticismType ct2 = new CriticismType()
        {
            CriticismTypeText = "Recommendation"
        };
        db.CriticismTypes.Add(ct2);
        CriticismType ct3 = new CriticismType()
        {
            CriticismTypeText = "Risk"
        };
        db.CriticismTypes.Add(ct3);
        Questionnaire q1 = new Questionnaire()
        {
            QuestionnaireName = "Security Check Light",
        };
        db.Questionnaires.Add(q1);
        Category c1 = new Category()
        {
            CategoryText = "99 Test"
        };
        db.Categories.Add(c1);
        db.SaveChanges();
    }
}