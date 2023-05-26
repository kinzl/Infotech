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
            PasswordHash = "6MTgt6csLPfst+ON2fOVaEYbhfgYHe44M4ZBOS7qwCpcF0rUBR3EPfh0wsOHJHLFIcdsPkZaynDZc64uIy7awg==",
            PasswordSalt = "ZlD9i+XCC97K/xNwLdLFcokqYNfa6NSqQ6kOTsxCtSVZA2INYaFjhAwfk3UQCe9UtFEHgAKJORpNeDCafr60iA=="
        };
        //pw = admin
        db.UserNames.Add(admin);
        UserName kinzle = new UserName
        {
            IsAdmin = false,
            Username = "kinzle",
            PasswordHash = "fr0c/l2J0T0aEH97GleMLx9gofiYEmOUmGO7WJFreFLMy42Q1aBeRl7YN1LDQMo9S2pdThFfiZhfsjA1Pgalsg==",
            PasswordSalt = "AeRU+Bh/KnQq/iX7+klYqPOD9OkvGyNgTFIUmVuWbYyJnx691TIcpH2bYWJ5YCIewuLEb5ygDZqqpWFDjo85Hw=="
        };
        //pw = emilk
        db.UserNames.Add(kinzle);
        UserName davk = new UserName
        {
            IsAdmin = false,
            Username = "davk",
            PasswordHash = "9316F2B4B3EE2AFE5D429C907B7A08C464AC9D5381CB9C794CD4D7448D989341A8D7A285BA21A8564115E5E9C4AC39047F982AE118E27970F6F8F45FACC1B851",
            PasswordSalt = "F6D1A4CF1CA463920D8D9534B414B9C19263B64019493A1CFE1E4E94780B9CD5ECE2A4815A8E87087F654368F4B6BE3041ABA48E5762CDE56D9BC05373EF17A9"
        };
        //dav funktioniert incht
        //pw = IchbinDavK#1
        db.UserNames.Add(davk);
        UserName ich = new UserName
        {
            IsAdmin = false,
            Username = "ich",
            PasswordHash = "+yl49EVwtEMhNMrfygFuJtnBRyhvu3vW6SNig2TXdT+R8i3t53C+9C8bwPUO6xZDYGbDxCVk/S6x7k7gyywo7A==",
            PasswordSalt = "vskwmQEjyLR3NM8B9D4iEhi5j+rSFMPxDB0AKZg1G96D9PM8EwwJIR6HVS/K1u5e2DCEaZEgjF6LYN43ZhsUdw=="
        };
        //pw = ich
        db.UserNames.Add(ich);
        db.SaveChanges();
   
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