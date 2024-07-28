using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using TagHelpers.Extensions;

namespace TagHelpers
{
    [HtmlTargetElement("generic", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GenericTagHelper : TagHelper
    {

        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        //[HtmlAttributeName("isHtml")]
        //public bool IsHtml { get; set; } = true;

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
            var propValue = AspFor.Model;            

            output.Content.SetHtmlContent(propValue.ToString());
        }
    }
}
