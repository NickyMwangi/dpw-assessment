using Library.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Library.Utility
{
    public static class ExceptionExtension
    {
        // Log an Exception 
        public static void LogException(this Exception exc, string source = null)
        {
            string userName = string.Empty;
            var excSource = StaticContextHelper.Current.Request.Path.ToString();
            if (string.IsNullOrWhiteSpace(source))
                excSource += $" - {source}";

            if (StaticContextHelper.Current.User != null)
                userName = StaticContextHelper.Current.User.Identity.Name;
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 
            string folder = StaticContextHelper.GetAppSetting("FilePath:ErrorLog");
            folder = ((IHostingEnvironment)StaticContextHelper.Current.RequestServices.GetService(typeof(IHostingEnvironment))).ContentRootPath;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filename = string.Concat("ErrorLog", DateTime.Today.ToString("yyyyMMdd"), ".txt");

            string logFile = Path.Combine(folder, filename);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            sw.WriteLine("User: " + userName);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + excSource);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        public static string AlertMessage(this Exception ex, bool writeLog = true)
        {
            //if (writeLog)
            //    LogException(ex);
            var displayMsg = ex.Message;
            if (ex.InnerException != null)
            {
                displayMsg = ex.InnerException.Message;
                if (!string.IsNullOrEmpty(displayMsg) && displayMsg.TrimStart().StartsWith("<"))
                {
                    try
                    {
                        var msg = XElement.Parse(displayMsg);
                        displayMsg = msg.Value;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            var returnMsg = Regex.Replace(displayMsg, @"(?<=\w)\r\n(?=\w)", " ", RegexOptions.Compiled);
            returnMsg = Regex.Replace(returnMsg, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
            returnMsg ??= "Unspecified Exception Occurred";
            return returnMsg.Substring(0, Math.Min(250, returnMsg.Length));
        }

        public static string AlertMessage(this string message, bool writeLog = false)
        {
            //if (writeLog)
            //{
            //    var ex = new Exception(message);
            //    LogException(ex);
            //}
            var returnMsg = Regex.Replace(message, @"(?<=\w)\r\n(?=\w)", " ", RegexOptions.Compiled);
            returnMsg = Regex.Replace(returnMsg, "[^a-zA-Z0-9_.]+", " ", RegexOptions.Compiled);
            returnMsg ??= "Unspecified Exception Occurred";
            return returnMsg.Substring(0, Math.Min(250, returnMsg.Length));
        }

        public static string AlertMessage(this ModelStateDictionary modelState, bool writeLog = false)
        {
            var message = string.Join(";", modelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage));
            if (writeLog)
            {
                var ex = new Exception(message);
                LogException(ex);
            }
            var returnMsg = Regex.Replace(message, @"(?<=\w)\r\n(?=\w)", " ", RegexOptions.Compiled);
            returnMsg = Regex.Replace(returnMsg, "[^a-zA-Z0-9_.-;:]+", " ", RegexOptions.Compiled);
            returnMsg ??= "Unspecified Exception Occurred";
            return returnMsg.Substring(0, Math.Min(250, returnMsg.Length));
        }
    }
}
