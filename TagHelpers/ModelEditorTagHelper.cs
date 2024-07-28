using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TagHelpers
{
    [HtmlTargetElement("model-editor", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class ModelEditorTagHelper : TagHelper
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly HtmlEncoder _htmlEncoder;
        public ModelEditorTagHelper(IHtmlHelper htmlHelper, HtmlEncoder htmlEncoder)
        {
            _htmlHelper = htmlHelper as HtmlHelper;
            _htmlEncoder = htmlEncoder;

        }

        [ViewContext]
        public ViewContext ViewContext
        {
            set => _htmlHelper.Contextualize(value);
        }

        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName("model-partial")]
        public string ModelPartial { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var modelName = new ViewDataDictionary(_htmlHelper.ViewData) { { "ModelName", AspFor.Name } };

            var partial = await _htmlHelper.PartialAsync(ModelPartial, AspFor.Model, modelName);
            var writer = new StringWriter();
            partial.WriteTo(writer, _htmlEncoder);

            output.Content.SetHtmlContent(writer.ToString());
        }
    }
}
