using System.Data;
using System.Reflection.Emit;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Questionnaire_Frontend.Dto;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class IndexModel : PageModel
{
    public string? ErrorText;
    public string Username = "Admin";
    public string Password = "admin";

    public string InputUsername { get; set; }
    public string InputPassword { get; set; }

    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public IndexModel(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
        
         //db.Database.EnsureDeleted();
         //db.Database.EnsureCreated();
        try
        {
             //SeederExtension.Seed(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
        }
    }

    public void OnGet(string errorText)
    {
        ErrorText = errorText;
    }

    public IActionResult OnPostLogin(LoginDto body)
    {
        /*if (body.Username == Username && body.Password == Password)
        {
            return new RedirectToPageResult("MainWindow");
        }
        else
        {
            // return new RedirectToPageResult("Index", new { ErrorText = "Username or Password is wrong" });
            return new RedirectToPageResult("MainWindow");
        }*/





        //ToDo: Convert password to salt and hash and compare them
        string uName = "";
        try
        {
            uName = _db.UserNames.Where(x => x.Username == body.Username.ToString()).Select(x => x.Username).First();

        }
        catch (Exception ex)
        {
            return new RedirectToPageResult("Index", new { ErrorText = "Password or Username is wrong" });
            Console.WriteLine("Gibt keinen Username"); 
        }
        //pw funktioniert alles nurnoch schauen wie man macht um neue user in db anlegen

        if (uName == body.Username.ToString())
        {
            Console.WriteLine("Username: "+body.Username.ToString());
            //nur das, pe und securitycheckcontext (on model creating ) geändert
            //zuständig um Passwort zu verschlpsseln
            PasswordEncryption pe = new PasswordEncryption();//um pw zu encrypten
            /*var hash = pe.HashPasword(body.Password.ToString(), out var salt);
            Console.WriteLine($"Password: {body.Password.ToString()}");
            Console.WriteLine($"Password hash: {hash}");
            Console.WriteLine($"Generated salt: {Convert.ToHexString(salt)}");*/
            
            //um encryptetes pw zu vergleichen 
            //var hash1 = pe.HashPasword(body.Password.ToString(), out var salt1);
            //pw sollte das eigentliche passwort sein, aber probleme, denn wir haben nur hash und salt
            string pwhash = _db.UserNames.Where(x => x.Username == body.Username.ToString()).Select(x=>x.PasswordHash).First();
            Console.WriteLine("Hash von db= "+pwhash);
            string pwsalt = _db.UserNames.Where(x => x.Username == body.Username.ToString()).Select(x=>x.PasswordSalt).First();
            byte[] bytes = Encoding.ASCII.GetBytes(pwsalt);
            if (pe.VerifyPassword(body.Password.ToString(), pwhash, bytes))
            {
                Console.WriteLine("Passwort stimmt überein");
                return new RedirectToPageResult("MainWindow");
            }
            else
            {
                Console.WriteLine("Passwort falsch");
                return new RedirectToPageResult("Index", new { ErrorText = "Password or Username is wrong"});
            }
        }
        else
        {
            Console.WriteLine("Gibt keinen User: "+body.Username.ToString());
        }







        return null;
        /*if (_db.UserNames.Where(x => x.Username == body.Username && x.PasswordHash == body.Password).Count() == 1)
        {
            return new RedirectToPageResult("MainWindow");
        }*/
    }
}