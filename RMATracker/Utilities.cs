using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Mvc;

public static class Utilities
    
    {
     public static string test;
    public static List<SelectListItem> GetHeartBeatStates()
    {
        return new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Fast Blinking Green" , Text = "Fast Blinking Green"  },
            new SelectListItem() { Value = "Solid Green" , Text = "Solid Green" },
            new SelectListItem() { Value = "Off" , Text = "Off" }
        };
    }
    public static List<SelectListItem> GetCableOrigins()
    {
        return new List<SelectListItem>()
        {
            new SelectListItem() { Value = "0" , Text = "Previous Card Access System install"  },
            new SelectListItem() { Value = "1" , Text = "the typical ACS-1E 6 conductor 22 Ga."  },
            new SelectListItem() { Value = "2" , Text = "18 Ga. Stranded Cable in a Plenum Jacket"  }
        };
    }
    public static List<SelectListItem> GetAlarmStates()
    {
        return new List<SelectListItem>()
        {
            new SelectListItem() { Value = "Red" , Text = "Red"  },
            new SelectListItem() { Value = "Off" , Text = "Off" }
        };
    }
    public static List<SelectListItem> GetStates()
    {
        return new List<SelectListItem>()
        {
            new SelectListItem() {Text="Alabama", Value="AL"},
            new SelectListItem() { Text="Alaska", Value="AK"},
            new SelectListItem() { Text="Arizona", Value="AZ"},
            new SelectListItem() { Text="Arkansas", Value="AR"},
            new SelectListItem() { Text="California", Value="CA"},
            new SelectListItem() { Text="Colorado", Value="CO"},
            new SelectListItem() { Text="Connecticut", Value="CT"},
            new SelectListItem() { Text="District of Columbia", Value="DC"},
            new SelectListItem() { Text="Delaware", Value="DE"},
            new SelectListItem() { Text="Florida", Value="FL"},
            new SelectListItem() { Text="Georgia", Value="GA"},
            new SelectListItem() { Text="Hawaii", Value="HI"},
            new SelectListItem() { Text="Idaho", Value="ID"},
            new SelectListItem() { Text="Illinois", Value="IL"},
            new SelectListItem() { Text="Indiana", Value="IN"},
            new SelectListItem() { Text="Iowa", Value="IA"},
            new SelectListItem() { Text="Kansas", Value="KS"},
            new SelectListItem() { Text="Kentucky", Value="KY"},
            new SelectListItem() { Text="Louisiana", Value="LA"},
            new SelectListItem() { Text="Maine", Value="ME"},
            new SelectListItem() { Text="Maryland", Value="MD"},
            new SelectListItem() { Text="Massachusetts", Value="MA"},
            new SelectListItem() { Text="Michigan", Value="MI"},
            new SelectListItem() { Text="Minnesota", Value="MN"},
            new SelectListItem() { Text="Mississippi", Value="MS"},
            new SelectListItem() { Text="Missouri", Value="MO"},
            new SelectListItem() { Text="Montana", Value="MT"},
            new SelectListItem() { Text="Nebraska", Value="NE"},
            new SelectListItem() { Text="Nevada", Value="NV"},
            new SelectListItem() { Text="New Hampshire", Value="NH"},
            new SelectListItem() { Text="New Jersey", Value="NJ"},
            new SelectListItem() { Text="New Mexico", Value="NM"},
            new SelectListItem() { Text="New York", Value="NY"},
            new SelectListItem() { Text="North Carolina", Value="NC"},
            new SelectListItem() { Text="North Dakota", Value="ND"},
            new SelectListItem() { Text="Ohio", Value="OH"},
            new SelectListItem() { Text="Oklahoma", Value="OK"},
            new SelectListItem() { Text="Oregon", Value="OR"},
            new SelectListItem() { Text="Pennsylvania", Value="PA"},
            new SelectListItem() { Text="Rhode Island", Value="RI"},
            new SelectListItem() { Text="South Carolina", Value="SC"},
            new SelectListItem() { Text="South Dakota", Value="SD"},
            new SelectListItem() { Text="Tennessee", Value="TN"},
            new SelectListItem() { Text="Texas", Value="TX"},
            new SelectListItem() { Text="Utah", Value="UT"},
            new SelectListItem() { Text="Vermont", Value="VT"},
            new SelectListItem() { Text="Virginia", Value="VA"},
            new SelectListItem() { Text="Washington", Value="WA"},
            new SelectListItem() { Text="West Virginia", Value="WV"},
            new SelectListItem() { Text="Wisconsin", Value="WI"},
            new SelectListItem() { Text="Wyoming", Value="WY"}
        };
    }
    public static string ViewToString(Controller controller, string viewName, object model)
    {
        controller.ViewData.Model = model;
        using (StringWriter sw = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            viewResult.View.Render(viewContext, sw);
            return sw.ToString();
        }
    }
}

