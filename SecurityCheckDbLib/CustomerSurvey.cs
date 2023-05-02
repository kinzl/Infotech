using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCheckDbLib;
public class CustomerSurvey
{
    public int CustomerSurveyId { get; set; }
    public SurveyQuestion SurveyQuestion { get; set; }
    public string? CompanyName { get; set; }
}

