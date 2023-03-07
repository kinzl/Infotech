using Microsoft.AspNetCore.Mvc;

namespace Questionnaire_Frontend.Controller;

public class LoginController : Microsoft.AspNetCore.Mvc.Controller
{
    // 
    // GET: /HelloWorld/ 
 
    public string Index() 
    { 
        return "This is my <b>default</b> action..."; 
    } 
 
    // 
    // GET: /HelloWorld/Welcome/ 
 
    public string Welcome() 
    { 
        return "This is the Welcome action method..."; 
    } 
}