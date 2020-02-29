using System;
using System.Web;
using System.Web.Mvc;

public class RequestSwitcherAttribute : ActionFilterAttribute
{
    string desktopHost, mobileHost;
    public RequestSwitcherAttribute(string desktopHost, string mobileHost)

    {
        if (string.IsNullOrWhiteSpace(desktopHost) || string.IsNullOrWhiteSpace(mobileHost))
            throw new ArgumentNullException("Host or MobileHost");
        this.desktopHost = desktopHost;
        this.mobileHost = mobileHost;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)

    {
        var context = filterContext.HttpContext;
        var request = context.Request;
        var port = request.Url.Port;
        var host = request.UserHostName;
        var device = System.Web.WebPages.BrowserHelpers.GetOverriddenBrowser(context);
        if ((device.IsMobileDevice && host != mobileHost) || (!device.IsMobileDevice && host != desktopHost))
        {
            string url = (request.IsSecureConnection ? "https://" : "http://") +
                         (device.IsMobileDevice ? mobileHost : desktopHost) +
                         ((port == 80 || port == 443) ? "" : ":" + port) + request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }
    }

}
