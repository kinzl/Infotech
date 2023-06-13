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
        _logger.LogInformation("Index OnGet");
        ErrorText = errorText;
    }

    public async Task<IActionResult> OnPostLogin(LoginDto body)
    {
        _logger.LogInformation("Index OnPostLogin");
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

        try
        {
            if (uName == body.Username)
            {
                PasswordEncryption pe = new PasswordEncryption();//um pw zu encrypten
                
                string pwhash = _db.UserNames.Where(x => x.Username == body.Username).Select(x => x.PasswordHash).First();
                string pwsalt = _db.UserNames.Where(x => x.Username == body.Username).Select(x => x.PasswordSalt).First();
                if (pe.VerifyPassword(body.Password.ToString(), pwhash, pwsalt))
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }
        return new RedirectToPageResult("Index", new { ErrorText = "Password or Username is wrong" });
        /*if (_db.UserNames.Where(x => x.Username == body.Username && x.PasswordHash == body.Password).Count() == 1)
        {
            return new RedirectToPageResult("MainWindow");
        }*/
    }
}