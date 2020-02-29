using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace RMATracker
{
    public class MobileDisplayMode : DefaultDisplayMode
    {
        public static readonly List<string> MobileList = new List<string>
    {
        "Android",
        "Mobile",
        "Opera Mobi",
        "Samsung",
        "HTC",
        "Nokia",
        "Ericsson",
        "SonyEricsson",
        "iPhone"
        ,"ipod"
        , "symbian"
         ,"android"
                        ,"windows ce"
                        ,"blackberry"
                        ,"palm"
                        ,"opera mini"
    };

        public MobileDisplayMode()
            : base("Mobile")
        {
            ContextCondition = (context => IsMobile(context, context.GetOverriddenUserAgent()));
        }

        private static bool IsMobile(HttpContextBase context, string useragentString)
        {
            return context.Request.Browser.IsMobileDevice || MobileList.Any(val => useragentString.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) >= 0);
        }
    }
}