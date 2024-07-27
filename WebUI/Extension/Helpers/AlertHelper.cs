using Library.Utility;
using Service.Utility.Extensions;
using System;

namespace WebUI.Extensions.Helpers
{
    public static class AlertHelper
    {
        public static string Swal_Message(this AlertEnum alert, string message = null, string title = null, bool? writeLog = null )
        {
            string icon;
            switch (alert)
            {
                case AlertEnum.success:
                    icon = Enum.GetName(typeof(AlertEnum), 0);
                    message ??= "";
                    title ??= "Done";
                    break;
                case AlertEnum.error:
                    icon = Enum.GetName(typeof(AlertEnum), 1);
                    message ??= "Error Occurred";
                    title ??= "Alert";
                    writeLog ??= true;
                    break;
                case AlertEnum.warning:
                    icon = Enum.GetName(typeof(AlertEnum), 2);
                    message ??= "";
                    title ??= "Please Note";
                    break;
                case AlertEnum.question:
                    icon = Enum.GetName(typeof(AlertEnum), 3);
                    message ??= "";
                    title ??= "Question";
                    break;
                default:
                    icon = Enum.GetName(typeof(AlertEnum), 4);
                    message ??= "";
                    title ??= "Information";
                    break;
            }
            string alertMsg = message.AlertMessage(writeLog??false);
            string swMsg = string.Format("Common.swalShow('{0}','{1}','{2}');", icon, title, alertMsg);
            return swMsg;
        }

        public static string Toastr_Message(this AlertEnum alert, string message)
        {
            return string.Format("Common.toastrShow('{0}','{1}');", Enum.GetName(typeof(AlertEnum), alert), message);
        }
    }
   
}
