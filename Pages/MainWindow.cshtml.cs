using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Questionnaire_Frontend.Pages;

public class MainWindow : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<int> list = new() { 1, 2, 3, 4, 5 };

    public MainWindow(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public void OnGetRedirectShowExistingData()
    {
        Response.Redirect("AnswerQuestions");

    }

    // protected void Page_Load(object sender, EventArgs e)
    // {
    //
    // }
    // protected void BTNSubmit_Click(object sender, EventArgs e)
    // {
    //     FindControl(RDBTNTBL);
    // }
    // public void FindControl(Control controls)
    // {
    //     var radioButton = new RadioButton();
    //     foreach(Control c in controls.Controls)
    //     {
    //         if (c.HasControls())
    //         {
    //             FindControl(c);
    //         }
    //         else if (c.GetType().ToString().Equals("System.Web.UI.WebControls.RadioButton"))
    //         {
    //             if ((c as RadioButton).Checked)
    //             {
    //                 Response.Wri
    //                 Response.Write();
    //                 Response.Write(c.ID);
    //                 return;
    //             }
    //         }
    //
    //     }
    // }
}