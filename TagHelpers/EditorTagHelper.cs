using Data.Entities.DropdownList;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using TagHelpers.Extensions;

namespace TagHelpers
{
    [HtmlTargetElement("editor", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class EditorTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression AspFor { get; set; }

        [HtmlAttributeName("editor-id")]
        public string EditorId { get; set; }

        [HtmlAttributeName("editor-class")]
        public string EditorClass { get; set; }

        [HtmlAttributeName("has-label")]
        public bool HasLabel { get; set; } = true;

        [HtmlAttributeName("readonly")]
        public bool Readonly { get; set; } = false;

        [HtmlAttributeName("disabled")]
        public bool Disabled { get; set; } = false;

        [HtmlAttributeName("is-required")]
        public bool IsRequired { get; set; } = false;

        [HtmlAttributeName("is-hidden")]
        public bool Hidden { get; set; } = false;

        [HtmlAttributeName("placeholder")]
        public string Placeholder { get; set; }

        [HtmlAttributeName("autocomplete")]
        public bool AutoComplete { get; set; } = false;

        [HtmlAttributeName("option-label")]
        public string OptionLabel { get; set; } = "Select...";

        [HtmlAttributeName("select-list")]
        public IEnumerable<SelectListItem> SelectList { get; set; }

        [HtmlAttributeName("list-input")]
        public ListInputModel ListInput { get; set; }

        [HtmlAttributeName("target-ctrl")]
        public string TargetCtrl { get; set; }

        [HtmlAttributeName("target-ctrl2")]
        public string TargetCtrl2 { get; set; }

        [HtmlAttributeName("target-url")]
        public string TargetUrl { get; set; }

        [HtmlAttributeName("min-input")]
        public int MinInput { get; set; } = 0;

        [HtmlAttributeName("multi-select-list")]
        public MultiSelectList MultiSelect { get; set; }

        [HtmlAttributeName("select-multiple")]
        public bool SelectMultiple { get; set; } = false;

        [HtmlAttributeName("from-list")]
        public bool FromList { get; set; } = false;

        [HtmlAttributeName("control-type")]
        public string ControlType { get; set; }

        [HtmlAttributeName("height")]
        public string Height { get; set; } = "100";

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; set; }

        public EditorTagHelper(IHtmlGenerator _generator)
        {
            Generator = _generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var parent = new TagBuilder("div");
            parent.AddCssClass("form-group-sm");
            EditorAttributes(parent);
            var input = GenerateEditor();
            parent.InnerHtml.AppendHtml(input);
            EditorValidator(parent);
            output.Content.SetHtmlContent(parent);
            base.Process(context, output);
        }

        public void EditorAttributes(TagBuilder tag)
        {
            if (HasLabel && !Hidden && !FromList)
            {
                var label = GenerateEditorLabel();
                tag.InnerHtml.AppendHtml(label);
            }
            if (FromList && !string.IsNullOrWhiteSpace(EditorId))
            {
                EditorId = $"{EditorId}.{AspFor.Name}";
            }
            if (FromList && AspFor.Metadata.IsRequired)
            {
                Placeholder = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes("\u2731")));
            }
        }

        public void EditorValidator(TagBuilder tag)
        {
            var propType = AspFor.Metadata.TemplateHint ?? "";
            if (!propType.Equals("quill", StringComparison.OrdinalIgnoreCase) && !FromList)
            {
                var validation = Generator.GenerateValidationMessage(ViewContext, AspFor.ModelExplorer,
                    AspFor.Name, string.Empty, string.Empty, new { @class = "text-danger" });
                tag.InnerHtml.AppendHtml(validation);
            }
        }

        public TagBuilder GenerateEditorLabel()
        {
            var propName = EditorId ?? AspFor.Name;
            var labelText = AspFor.Metadata.DisplayName ?? AspFor.Metadata.PropertyName.CamelCaseToTitle();

            var label = Generator.GenerateLabel(ViewContext, AspFor.ModelExplorer,
                propName, labelText, new { @class = "control-label" });
            if ((AspFor.Metadata.IsRequired || IsRequired) && !Readonly && !Disabled)
            {
                label.InnerHtml.AppendHtml("<span class=\"text-danger\">&nbsp;&#x2731</span>");
            }


            label.MergeAttribute("id", string.Format("{0}_Label", EditorId ?? propName), true);
            return label;
        }

        public TagBuilder GenerateEditor()
        {
            var propType = AspFor.Metadata.TemplateHint ?? AspFor.Metadata.DataTypeName ?? AspFor.Metadata.UnderlyingOrModelType.Name;
            if (!string.IsNullOrWhiteSpace(ControlType))
                propType = ControlType;
            var propFormatString = AspFor.Metadata.EditFormatString;
            TagBuilder tagBuilder;
            switch (propType.ToLower())
            {
                case "decimal":
                    //propFormatString ??= "{0:n2}";
                    tagBuilder = GenerateTextInput(null, "decimal");
                    break;
                case "datetime":
                    propFormatString ??= "{0:dd MMM yyyy}";
                    tagBuilder = GenerateDateTime(propFormatString);
                    break;
                case "dropdown":
                case "option":
                    tagBuilder = GenerateDropDown();
                    break;
                case "textarea":
                    tagBuilder = GenerateMultiLineText();
                    break;
                case "boolean":
                    tagBuilder = GenerateCheckInput();
                    break;
                case "password":
                    tagBuilder = GenerateTextInput(propFormatString, "password");
                    break;
                case "emailaddress":
                    tagBuilder = GenerateTextInput(propFormatString, "email");
                    break;
                case "multiselect":
                    tagBuilder = GenerateMultiselect();
                    break;
                case "ajaxdropdown":
                    tagBuilder = GenerateAjaxDropDown();
                    break;
                case "imagedisplay":
                    tagBuilder = GenerateImageDisplay();
                    break;
                default:
                    tagBuilder = GenerateTextInput(propFormatString);
                    break;
            }
            return tagBuilder;

        }
        public TagBuilder GenerateTextInput(string formatString = null, string dataType = "text")
        {
            var propName = EditorId ?? AspFor.Name;
            var propValue = AspFor.Model;
            var textbox = Generator.GenerateTextBox(ViewContext, AspFor.ModelExplorer,
               propName, propValue, formatString, new { @class = string.Format("form-control form-control-sm {0}", EditorClass) });
            if (!string.IsNullOrWhiteSpace(EditorId))
                textbox.MergeAttribute("id", EditorId, true);
            if (Hidden)
            {
                textbox.MergeAttribute("type", "hidden", true);
                return textbox;
            }
            if (dataType != "text")
            {
                textbox.MergeAttribute("type", dataType, true);
            }
            if (dataType == "decimal")
            {
                textbox.MergeAttribute("data-mask", dataType, true);
            }
            if (!AutoComplete)
                textbox.MergeAttribute("autocomplete", "off", true);
            if (!string.IsNullOrWhiteSpace(Placeholder))
                textbox.MergeAttribute("placeholder", Placeholder, true);
            if (Readonly)
                textbox.MergeAttribute("readonly", "readonly", true);
            if (Disabled)
                textbox.MergeAttribute("disabled", "disabled", true);
            if (IsRequired && !Readonly && !Disabled)
                textbox.MergeAttribute("required", "required");

            return textbox;
        }

        public TagBuilder GenerateDateTime(string formatString = null)
        {
            var propName = EditorId ?? AspFor.Name;
            var propValue = AspFor.Model;
            var inputGrpId = ($"dp_{EditorId ?? propName}").Replace('[', '_').Replace(']', '_').Replace('.', '_');

            var textbox = Generator.GenerateTextBox(ViewContext, AspFor.ModelExplorer,
               propName, propValue, formatString, new
               {
                   type = "text",
                   @class = string.Format("form-control form-control-sm datetimepicker-input {0}", EditorClass),
                   data_target = "#" + inputGrpId,
                   data_toggle = "datetimepicker"
               });
            if (!string.IsNullOrWhiteSpace(EditorId))
                textbox.MergeAttribute("id", EditorId, true);
            if (Hidden)
            {
                textbox.MergeAttribute("type", "hidden", true);
                return textbox;
            }
            if (Readonly)
            {
                textbox.MergeAttribute("readonly", "readonly", true);
                return textbox;
            }
            if (Disabled)
            {
                textbox.MergeAttribute("disabled", "disabled", true);
                return textbox;
            }
            textbox.MergeAttribute("autocomplete", "off", true);
            textbox.MergeAttribute("placeholder", "DD MMM YYYY", true);
            if (IsRequired)
                textbox.MergeAttribute("required", "required");

            var iconCalender = new TagBuilder("i");
            iconCalender.AddCssClass("fa fa-calendar");

            var divIcon = new TagBuilder("div");
            divIcon.AddCssClass("input-group-text");

            var divButton = new TagBuilder("div");
            divButton.AddCssClass("input-group-append");
            divButton.MergeAttribute("data-target", "#" + inputGrpId);
            divButton.MergeAttribute("data-toggle", "datetimepicker");

            divIcon.InnerHtml.AppendHtml(iconCalender);
            divButton.InnerHtml.AppendHtml(divIcon);

            var divCalender = new TagBuilder("div");
            divCalender.AddCssClass("input-group date");
            divCalender.MergeAttribute("data-target-input", "nearest");
            divCalender.MergeAttribute("data-ctrl", "datepicker");
            divCalender.MergeAttribute("id", inputGrpId);
            divCalender.InnerHtml.AppendHtml(textbox);
            divCalender.InnerHtml.AppendHtml(divButton);

            return divCalender;
        }

        public TagBuilder GenerateDropDown()
        {
            var propName = EditorId ?? AspFor.Name;
            var propValue = AspFor.Model;
            var divId = string.Format("div_{0}", propName);
            IEnumerable<SelectListItem> ddlist = SelectList ?? new List<SelectListItem>();
            var dropdown = Generator.GenerateSelect(ViewContext, AspFor.ModelExplorer, OptionLabel ?? "",
               propName, ddlist, false, new
               {
                   @class = string.Format("form-control form-control-sm {0}", EditorClass),
                   data_ctrl = "select2",
                   data_value = propValue,
                   data_placeholder = OptionLabel
               });
            if (!string.IsNullOrWhiteSpace(EditorId))
                dropdown.MergeAttribute("id", EditorId, true);
            if (Hidden)
            {
                dropdown.MergeAttribute("type", "hidden", true);
                return dropdown;
            }
            if (Readonly)
                dropdown.MergeAttribute("readonly", "readonly", true);
            if (Disabled)
                dropdown.MergeAttribute("disabled", "disabled", true);
            if (IsRequired && !Readonly && !Disabled)
                dropdown.MergeAttribute("required", "required");
            if (SelectMultiple)
                dropdown.MergeAttribute("multiple", "multiple", true);

            var divdd = new TagBuilder("div");
            divdd.MergeAttribute("id", divId);
            divdd.InnerHtml.AppendHtml(dropdown);
            return divdd;
        }

        public TagBuilder GenerateAjaxDropDown()
        {
            var propName = EditorId ?? AspFor.Name;
            var propValue = AspFor.Model;
            var divId = string.Format("div_{0}", propName);
            List<SelectListItem> ddlist = new List<SelectListItem>();

            var dropdown = Generator.GenerateSelect(ViewContext, AspFor.ModelExplorer, OptionLabel ?? "",
               propName, new SelectList(ddlist, "Value", "Text", propValue), false, new
               {
                   @class = string.Format("form-control form-control-sm {0}", EditorClass),
                   data_ctrl = "ajaxselect",
                   data_value = propValue,
                   data_placeholder = OptionLabel
               });
            if (!string.IsNullOrWhiteSpace(EditorId))
                dropdown.MergeAttribute("id", EditorId, true);
            if (!string.IsNullOrWhiteSpace(ListInput.DataUrl))
                dropdown.MergeAttribute("data-ajax--url", ListInput.DataUrl, true);
            if (!string.IsNullOrWhiteSpace(TargetCtrl))
                dropdown.MergeAttribute("data-target-ctrl", TargetCtrl, true);
            if (!string.IsNullOrWhiteSpace(TargetCtrl2))
                dropdown.MergeAttribute("data-target-ctrl", TargetCtrl2, true);
            if (!string.IsNullOrWhiteSpace(TargetUrl))
                dropdown.MergeAttribute("data-target-url", TargetUrl, true);
            if (MinInput > 0)
                dropdown.MergeAttribute("data-minimum-input-length", MinInput.ToString(), true);
            if (Hidden)
            {
                dropdown.MergeAttribute("type", "hidden", true);
                return dropdown;
            }
            if (Readonly || ListInput.ReadOnly)
                dropdown.MergeAttribute("readonly", "readonly", true);
            if (Disabled)
                dropdown.MergeAttribute("disabled", "disabled", true);
            if (IsRequired && !Readonly && !Disabled)
                dropdown.MergeAttribute("required", "required");
            if (SelectMultiple)
                dropdown.MergeAttribute("multiple", "multiple", true);

            var divdd = new TagBuilder("div");
            divdd.MergeAttribute("id", divId);
            //dropdown.MergeAttribute("data-parent", divId);
            divdd.InnerHtml.AppendHtml(dropdown);
            return divdd;
        }

        public TagBuilder GenerateAjaxCascadeDropDown()
        {
            var propName = EditorId ?? AspFor.Name;
            var propValue = AspFor.Model;
            var divId = string.Format("div_{0}", EditorId ?? propName);
            IEnumerable<SelectListItem> ddlist = new List<SelectListItem>();

            var dropdown = Generator.GenerateSelect(ViewContext, AspFor.ModelExplorer, OptionLabel ?? "",
               propName, new SelectList(ddlist, "Value", "Text", propValue), false, new
               {
                   @class = string.Format("form-control form-control-sm {0}", EditorClass),
                   data_ctrl = "ajaxcascade",
                   data_value = propValue,
                   data_placeholder = OptionLabel,
                   data_target_ctrl = TargetCtrl,
                   data_target_url = TargetUrl
               });
            if (!string.IsNullOrWhiteSpace(EditorId))
                dropdown.MergeAttribute("id", EditorId, true);
            if (!string.IsNullOrWhiteSpace(ListInput.DataUrl))
                dropdown.MergeAttribute("data-ajax--url", ListInput.DataUrl, true);
            if (MinInput > 0)
                dropdown.MergeAttribute("data-minimum-input-length", MinInput.ToString(), true);
            if (Hidden)
            {
                dropdown.MergeAttribute("type", "hidden", true);
                return dropdown;
            }
            if (Readonly)
                dropdown.MergeAttribute("readonly", "readonly", true);
            if (Disabled)
                dropdown.MergeAttribute("disabled", "disabled", true);
            if (IsRequired && !Readonly && !Disabled)
                dropdown.MergeAttribute("required", "required");
            if (SelectMultiple)
                dropdown.MergeAttribute("multiple", "multiple", true);

            var divdd = new TagBuilder("div");
            divdd.MergeAttribute("id", divId);
            //dropdown.MergeAttribute("data-parent", divId);
            divdd.InnerHtml.AppendHtml(dropdown);
            return divdd;
        }

        public TagBuilder GenerateMultiselect()
        {
            var propName = EditorId ?? AspFor.Name;

            var multiselect = Generator.GenerateSelect(ViewContext, AspFor.ModelExplorer, "",
               propName, MultiSelect, true, new
               {
                   @class = string.Format("form-control form-control-sm {0}", EditorClass),
                   data_ctrl = "multiselect"
               });
            if (!string.IsNullOrWhiteSpace(EditorId))
                multiselect.MergeAttribute("id", EditorId, true);
            if (Hidden)
            {
                multiselect.MergeAttribute("type", "hidden", true);
                return multiselect;
            }
            if (Readonly)
                multiselect.MergeAttribute("readonly", "readonly", true);
            if (Disabled)
                multiselect.MergeAttribute("disabled", "disabled", true);
            return multiselect;
        }

        public TagBuilder GenerateCheckInput()
        {
            var propVal = AspFor.Model ?? false;
            if (propVal.ToString() != bool.TrueString && propVal.ToString() != bool.FalseString)
                propVal = false;

            var checkbox = Generator.GenerateCheckBox(ViewContext, AspFor.ModelExplorer,
               EditorId ?? AspFor.Name, (bool)propVal, new { @class = string.Format("form-control form-control-sm {0}", EditorClass) });
            if (!string.IsNullOrWhiteSpace(EditorId))
                checkbox.MergeAttribute("id", EditorId, true);
            if (Hidden)
            {
                checkbox.MergeAttribute("type", "hidden", true);
                return checkbox;
            }
            if (Readonly)
                checkbox.MergeAttribute("readonly", "readonly", true);
            if (Disabled || Readonly)
                checkbox.MergeAttribute("disabled", "disabled", true);
            if (IsRequired && !Readonly && !Disabled)
                checkbox.MergeAttribute("required", "required", true);

            var icb = new TagBuilder("i");
            icb.AddCssClass("cr-icon icofont icofont-ui-check txt-primary");

            var spcb = new TagBuilder("span");
            spcb.AddCssClass("cr");
            spcb.InnerHtml.AppendHtml(icb);

            var lblcb = new TagBuilder("label");
            lblcb.InnerHtml.AppendHtml(checkbox);
            lblcb.InnerHtml.AppendHtml(spcb);

            var divcb = new TagBuilder("div");
            divcb.AddCssClass("checkbox-fade fade-in-primary");
            divcb.InnerHtml.AppendHtml(lblcb);

            var divmaincb = new TagBuilder("div");
            divmaincb.InnerHtml.AppendHtml(divcb);
            return divmaincb;
        }

        public TagBuilder GenerateMultiLineText()
        {
            var propName = EditorId ?? AspFor.Name;

            var textarea = Generator.GenerateTextArea(ViewContext, AspFor.ModelExplorer, propName, int.MaxValue, int.MaxValue,
                new { @class = string.Format("form-control form-control-sm {0}", EditorClass), data_ctrl = "multiline" });
            if (!string.IsNullOrWhiteSpace(EditorId))
            {
                textarea.MergeAttribute("id", EditorId, true);
            }
            textarea.MergeAttribute("data-readonly", Readonly.ToString().ToLower(), true);
            textarea.MergeAttribute("data-height", Height, true);
            if (Hidden)
                textarea.AddCssClass("hidden");
            if (Disabled)
                textarea.MergeAttribute("disabled", "disabled", true);
            if (!AutoComplete)
                textarea.MergeAttribute("autocomplete", "off", true);
            if (!string.IsNullOrWhiteSpace(Placeholder))
                textarea.MergeAttribute("placeholder", Placeholder, true);
            if (IsRequired && !Readonly && !Disabled)
                textarea.MergeAttribute("required", "", true);

            return textarea;
        }

        public TagBuilder GenerateImageDisplay()
        {
            var propValue = AspFor.Model ?? "";

            var img = new TagBuilder("img");
            img.AddCssClass("img-fluid img-thumbnail");
            img.MergeAttribute("src", propValue.ToString(), true);
            img.MergeAttribute("alt", "", true);

            var divthumb = new TagBuilder("div");
            divthumb.AddCssClass("thumb");
            divthumb.InnerHtml.AppendHtml(img);

            var divmain = new TagBuilder("div");
            divmain.AddCssClass("thumbnail");
            divmain.InnerHtml.AppendHtml(divthumb);
            return divmain;
        }


    }
}