﻿namespace Questionnaire_Frontend.Pages;

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
        try
        {
            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
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

    public async Task<IActionResult> OnPostLogin(LoginDto body)
    {
        //Comment this line out if you are not in the test system
        return new RedirectToPageResult("MainWindow");
        
        
        string uName;
        try
        {
            uName = _db.UserNames.Where(x => x.Username == body.Username).Select(x => x.Username).Single();
        }
        catch (Exception ex)
        {
            return new RedirectToPageResult("Index", new { ErrorText = "Password or Username is wrong" });
        }
        //pw funktioniert alles nurnoch schauen wie man macht um neue user in db anlegen

        if (uName == body.Username)
        {
            Console.WriteLine("Username: "+body.Username);
            //nur das, pe und securitycheckcontext (on model creating ) geändert
            //zuständig um Passwort zu verschlüsseln
            PasswordEncryption pe = new PasswordEncryption();//um pw zu encrypten
            /*var hash = pe.HashPasword(body.Password.ToString(), out var salt);
            Console.WriteLine($"Password: {body.Password.ToString()}");
            Console.WriteLine($"Password hash: {hash}");
            Console.WriteLine($"Generated salt: {Convert.ToHexString(salt)}");*/
            
            //um encryptetes pw zu vergleichen 
            //var hash1 = pe.HashPasword(body.Password.ToString(), out var salt1);
            //pw sollte das eigentliche passwort sein, aber probleme, denn wir haben nur hash und salt
            string pwhash = _db.UserNames.Where(x => x.Username == body.Username).Select(x=>x.PasswordHash).First();
            Console.WriteLine("Hash von db= "+pwhash);
            string pwsalt = _db.UserNames.Where(x => x.Username == body.Username).Select(x=>x.PasswordSalt).First();
            byte[] bytes = Encoding.ASCII.GetBytes(pwsalt);
            if (pe.VerifyPassword(body.Password.ToString(), pwhash, bytes))
            {
                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, uName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = false
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);
                
                return new RedirectToPageResult("MainWindow");
            }
        }

        return new RedirectToPageResult("Index", new { ErrorText = "Password or Username is wrong"});
        /*if (_db.UserNames.Where(x => x.Username == body.Username && x.PasswordHash == body.Password).Count() == 1)
        {
            return new RedirectToPageResult("MainWindow");
        }*/
    }
}