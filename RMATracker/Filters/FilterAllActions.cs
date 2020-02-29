using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;

public class RedirectingAction : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        dynamic skip = context.HttpContext.Session.Contents["Skip"];
        if (skip??false == true)
        {
            context.HttpContext.Session.Contents["Skip"] = false;
            return;
        }

        base.OnActionExecuting(context);

        if (context.HttpContext.Session.Contents["Token"] == null)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Home",
                action = "Login"
            }));
            context.HttpContext.Session.Contents["Skip"] = true;
        }
    }
}
public class FilterAllActions : IActionFilter, IResultFilter
{
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        throw new System.NotImplementedException();
    }

    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
        throw new System.NotImplementedException();
    }

    public void OnResultExecuting(ResultExecutingContext filterContext)
    {
        throw new System.NotImplementedException();
    }

    public void OnResultExecuted(ResultExecutedContext filterContext)
    {
        throw new System.NotImplementedException();
    }
}