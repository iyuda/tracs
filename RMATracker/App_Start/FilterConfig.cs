using System.Web;
using System.Web.Mvc;

namespace RMATracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new FilterAllActions());
            //filters.Add(new RequestSwitcherAttribute("::1", "127.0.0.1"));
        }
    }
}
