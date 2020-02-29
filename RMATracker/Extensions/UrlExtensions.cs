using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Hosting;

namespace Kendo.Extensions
{
    public static class UrlExtensions
    {
        public static string Widget(this UrlHelper url, string widget)
        {
            return url.ExampleUrl(widget, "index");
        }

        public static string ExampleUrl(this UrlHelper url, string widget, string example)
        {
            var widgetUrl = url.Action(example, widget);

            if (example == "index" && !widgetUrl.EndsWith("index"))
            {
                widgetUrl += "/index";
            }

            return widgetUrl;
        }

        public static string Script(this UrlHelper url, string file)
        {
            return ResourceUrl(url, "js", file, IsAbsoluteUrl(file));
        }

        public static string Style(this UrlHelper url, string file, string theme, string commonFile)
        {
            return url.Style(file)
                .Replace("CURRENT_THEME", theme)
                .Replace("CURRENT_COMMON", commonFile);
        }

        public static string Style(this UrlHelper url, string file)
        {
            return ResourceUrl(url, "styles", file, IsAbsoluteUrl(file));
        }

        private static string ResourceUrl(UrlHelper url, string assetType, string file, bool isAbsoluteUrl)
        {
            var CDN_ROOT = ConfigurationManager.AppSettings["CDN_ROOT"];
            if (CDN_ROOT == "$CDN_ROOT")
            {
                if (assetType == "styles")
                {
                    return url.Content(string.Format("~/content/web/{0}", file));
                }

                return url.Content(string.Format("~/Scripts/{0}", file));
            }

            if (isAbsoluteUrl == true)
            {
                return file;
            }

            return url.Content(string.Format("{0}/{1}/{2}", CDN_ROOT, assetType, file));
        }

        public static bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }
    }
}
