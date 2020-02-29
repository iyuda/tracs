using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace System.Web.Mvc.Html
{
    public static class ImageHelper
    {

        public static MvcHtmlString Image(this HtmlHelper helper, string id, string altText, string height, string width)

        {

            var builder = new TagBuilder("img");

            builder.MergeAttribute("id", id);

            builder.MergeAttribute("alt", altText);

            builder.MergeAttribute("height", height);

            builder.MergeAttribute("width", width);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));

        }

    }
}