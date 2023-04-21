﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurityCheckDbLib;

namespace Questionnaire_Frontend.Pages;

public class MainWindow : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private SecurityCheckContext _db;

    public List<string> SecurityChecks = new()
    {
        "Company 1 | Date | Security Check Type | Progress",
        "Company 2 | Date | Security Check Type | Progress"
    };

    public int SelectedSecurityCheckIndex = 0;

    public MainWindow(ILogger<IndexModel> logger, SecurityCheckContext db)
    {
        _logger = logger;
        _db = db;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPostRedirectShowExistingData()
    {
        return new RedirectToPageResult("AnswerQuestions");
    }

    public IActionResult OnPostOpenSelectedCheck()
    {
        return new RedirectToPageResult("AnswerQuestions");
    }
    public void OnGetRedirectToUpdateSecurityCheck()
    {
        Response.Redirect("UpdateSecurityCheck");
    }
    
    public IActionResult OnGetRedirectChangePassword()
    {
        return new RedirectToPageResult("ChangePassword");
    }

    public void OnPostSecurityCheckList(string selectedItem)
    {
        for (int i = 0; i < SecurityChecks.Count; i++)
        {
            if (SecurityChecks[i] == selectedItem)
            {
                SelectedSecurityCheckIndex = i;
                break;
            }
        }
    }

    
}