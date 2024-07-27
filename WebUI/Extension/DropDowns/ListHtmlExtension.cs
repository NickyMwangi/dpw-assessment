using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

namespace WebUI.Extensions.DropDowns
{
    public static class ListHtmlExtension
    {
        public static string DisplayHtml(bool header = false, params string[] fieldValue)
        {
            var pcnt = fieldValue.Count();
            var maxSize = 1;
            var colclass = "col-sm-12";
            if (pcnt >= 6)
            {
                maxSize = 4;
                colclass = "col-sm-2";
            }
            else if (pcnt >= 4)
            {
                colclass = "col-sm-3";
                maxSize = 4;
            }
            else if (pcnt >= 3)
            {
                colclass = "col-sm-4";
                maxSize = 3;
            }
            else if (pcnt >= 2)
            {
                colclass = "col-sm-6";
                maxSize = 2;
            }
            var divmain = new TagBuilder("div");
            divmain.AddCssClass("col-sm-12");
            var divcol = new TagBuilder("div");
            divcol.AddCssClass("row");
            for (var i = 0; i <= maxSize - 1; i++)
            {
                var newdiv = new TagBuilder("div");
                newdiv.AddCssClass(colclass + " text-nowrap>");
                if (header)
                {
                    var btag = new TagBuilder("b");
                    btag.InnerHtml.Append(fieldValue[i]);
                    newdiv.InnerHtml.AppendHtml(btag);
                }
                else
                {
                    newdiv.InnerHtml.Append(fieldValue[i]);
                }
                divcol.InnerHtml.AppendHtml(newdiv);
            }
            divmain.InnerHtml.AppendHtml(divcol);
            var displayStr = GetHtmlString(divmain);
            return displayStr;
        }

        public static string ListHeader(this string listType)
        {
            string header = "";
            switch (listType)
            {
                case "SavingsList":
                    header = DisplayHtml(true, "No", "Name", "ID", "Product");
                    break;
                case "MembersList":
                    header = DisplayHtml(true, "No", "Name", "ID No");
                    break;
                case "CollateralList":
                    header = DisplayHtml(true, "No", "Account No", "Collateral", "Collateral Name");
                    break;
                case "CreditAccountList":
                    header = DisplayHtml(true, "No", "Name", "Product Name");
                    break;
                case "GLAccountList":
                    header = DisplayHtml(true, "No", "Name");
                    break;
                case "LoanList":
                    header = DisplayHtml(true, "No", "Name", "Product");
                    break;
                case "CustomerLookupList":
                    header = DisplayHtml(true, "No", "Name", "City", "Contact");
                    break;
                case "HREmployeesList":
                    header = DisplayHtml(true, "No", "First Name", "Middle Name", "Last Name");
                    break;
                case "SalesAndPurchasersList":
                    header = DisplayHtml(true, "Code", "Name");
                    break;
                case "AgentList":
                    header = DisplayHtml(true, "Agent Code", "Name");
                    break;
                case "BankBranchList":
                    header = DisplayHtml(true, "Branch Code", "Branch Name");
                    break;
                default:
                    break;
            }
            return header;
        }

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
