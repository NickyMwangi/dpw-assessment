using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using TagHelpers.Extensions;

namespace TagHelpers
{
    [HtmlTargetElement("display", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DisplayTagHelper : TagHelper
    {

        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName("edit-format")]
        public string EditFormat { get; set; }

        [HtmlAttributeName("isHtml")]
        public bool IsHtml { get; set; } = false;

        [HtmlAttributeName("from-list")]
        public bool FromList { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            var propType = AspFor.Metadata.UnderlyingOrModelType.Name.ToLower();
            var propValue = AspFor.Model;
            var displayText = (AspFor.Metadata.DisplayName ?? AspFor.Metadata.PropertyName.CamelCaseToTitle()) + " : ";

            string text = "";
            if (propValue != null)
            {
                if (propType == "datetime" && !string.IsNullOrEmpty(EditFormat))
                {
                    text = ((DateTime)propValue).ToString(EditFormat);
                }
                else if (propType == "datetime" && string.IsNullOrEmpty(EditFormat))
                {
                    text = ((DateTime)propValue).ToString("dd MMM yyyy");
                    if (text == "01 Jan 0001")
                        text = "";
                }
                else if (propType == "decimal")
                {
                    text = ((decimal)propValue).ToString("N2");
                }
                else
                {
                    text = propValue.ToString();
                }
            }

            var boldLbl = new TagBuilder("b");
            if (!FromList)
            {
                boldLbl.InnerHtml.Append(displayText);
            }

            var span = new TagBuilder("span");

            if (IsHtml)
            {
                span.InnerHtml.AppendHtml(text);
            }
            else
            {
                span.InnerHtml.AppendHtml(boldLbl);
                span.InnerHtml.Append(text);
            }


            output.Content.SetHtmlContent(span);
        }
    }
}
