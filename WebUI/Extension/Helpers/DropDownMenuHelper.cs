using Library.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace WebUI.Extensions.Helpers
{
    public static class DropDownMenuHelper
    {

        //public static string DropDownMenu(this List<ActionLinkModel> buttonLinks)
        //{
        //    var divddm = new TagBuilder("div");
        //    divddm.AddCssClass("dropdown");
        //    var btn = new TagBuilder("button");
        //    btn.AddCssClass("btn btn-outline-primary btn-mini dropdown-btn");
        //    btn.MergeAttribute("id", buttonLinks[0].LinkId);
        //    btn.MergeAttribute("type", "button");
        //    btn.MergeAttribute("data-toggle", "dropdown");
        //    var ecllipsis = new TagBuilder("i");
        //    ecllipsis.AddCssClass("fa fa-ellipsis-h");
        //    btn.InnerHtml.AppendHtml(ecllipsis);
        //    var divmenu = new TagBuilder("div");
        //    divmenu.AddCssClass("dropdown-menu");
        //    foreach (var link in buttonLinks)
        //    {
        //        var ddml = DropDownLink(link);
        //        divmenu.InnerHtml.AppendHtml(ddml);
        //    }
        //    divddm.InnerHtml.AppendHtml(btn);
        //    divddm.InnerHtml.AppendHtml(divmenu);
        //    var ddmenulist = GetHtmlString(divddm);

        //    return ddmenulist;
        //}

        //public static string DropDownMenu(this List<ActionLinkModel> buttonLinks, string name, string icon)
        //{
        //    var btn = new TagBuilder("button");
        //    btn.AddCssClass("btn btn-outline-primary btn-sm btn-pure dropdown-toggle");
        //    btn.MergeAttribute("id", name.NameToId());
        //    btn.MergeAttribute("type", "button");
        //    btn.MergeAttribute("data-toggle", "dropdown");
        //    var btnicon = new TagBuilder("i");
        //    btnicon.AddCssClass(icon);
        //    btn.InnerHtml.AppendHtml(btnicon);
        //    btn.InnerHtml.Append(name);
        //    var divmenu = new TagBuilder("div");
        //    divmenu.AddCssClass("dropdown-menu");
        //    var divcol = new TagBuilder("div");
        //    divcol.AddCssClass("col-md-12");
        //    var divrow = new TagBuilder("div");
        //    divrow.AddCssClass("row");
        //    foreach (var link in buttonLinks)
        //    {
        //        var divinner = new TagBuilder("div");
        //        var ddml = DropDownLink(link);
        //        divinner.InnerHtml.AppendHtml(ddml);
        //        divrow.InnerHtml.AppendHtml(divinner);
        //    }
        //    divcol.InnerHtml.AppendHtml(divrow);
        //    divmenu.InnerHtml.AppendHtml(divcol);
        //    var ddmenulist = GetHtmlString(btn);
        //    ddmenulist += GetHtmlString(divmenu);
        //    return ddmenulist;
        //}

        //public static TagBuilder DropDownLink(ActionLinkModel link)
        //{
        //    var context = StaticContextHelper.Current;

        //    var urlArea = context.Request.RouteValues["area"].ToString();
        //    var urlController = context.Request.RouteValues["controller"].ToString();

        //    var linkUrl = @$"/{link.LinkController ?? urlController}";
        //    if (!string.IsNullOrWhiteSpace(link.LinkArea ?? urlArea))
        //        linkUrl = @$"/{link.LinkArea ?? urlArea}/{link.LinkController ?? urlController}";
        //    linkUrl += @$"/{link.LinkAction}";
        //    var query = $"?id={link.LinkId}";
        //    if(link.LinkRoute!=null && link.LinkRoute.Count != 0)
        //    {
        //        var routeStr = string.Join("&", link.LinkRoute.Select(x => x.Key + "=" + x.Value));
        //        query += $"&{routeStr}";
        //    }

        //    linkUrl = GetLinkUrl(linkUrl, query);

        //    var actionBtn = new TagBuilder("a");
        //    actionBtn.AddCssClass($"dropdown-item waves-light waves-effect {link.LinkClass}");
        //    actionBtn.MergeAttribute("href", linkUrl);
        //    //actionBtn.MergeAttribute("data-load", " ");
        //    if (link.LinkDataAttr != null && link.LinkDataAttr.Count != 0)
        //    {
        //        foreach(var attr in link.LinkDataAttr)
        //        {
        //            actionBtn.MergeAttribute(attr.Key, attr.Value.ToString(),true);
        //        }
        //    }
        //    var icon = new TagBuilder("i");
        //    icon.AddCssClass(link.LinkIcon);            
        //    actionBtn.InnerHtml.AppendHtml(icon);
        //    actionBtn.InnerHtml.Append(link.LinkName);
        //    return actionBtn;
        //}

        //public static string GetLinkUrl(string url, string query)
        //{
        //    var context = StaticContextHelper.Current;
        //    UriBuilder uriBuilder = new UriBuilder();
        //    uriBuilder.Scheme = context.Request.Scheme;
        //    uriBuilder.Host = context.Request.Host.Host;
        //    if (context.Request.Host.Port.HasValue)
        //        uriBuilder.Port = context.Request.Host.Port.Value;
        //    uriBuilder.Path = url;
        //    uriBuilder.Query = query;
        //    var encUri = uriBuilder.Uri.AbsoluteUri;
        //    return encUri;
        //}

        public static string GetHtmlString(IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
