
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Library.Utility;

public class ExceptionUtility
{

    // Log an Exception 
    public static void LogException(Exception exc, string source = null)
    {
        string userName = string.Empty;

        var filename = string.Concat("ErrorLog", DateTime.Today.ToString("yyyyMMdd"), ".txt");

        string logFile = Path.Combine("", filename);

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
        sw.WriteLine("Source: " + source);
        sw.WriteLine("Stack Trace: ");
        if (exc.StackTrace != null)
        {
            sw.WriteLine(exc.StackTrace);
            sw.WriteLine();
        }
        sw.Close();
    }

    public static string NavException(Exception ex)
    {
        ExceptionUtility.LogException(ex);
        var displayMsg = ex.Message;
        if (ex.InnerException != null)
        {
            displayMsg = ex.InnerException.Message;
            if (!string.IsNullOrEmpty(displayMsg) && displayMsg.TrimStart().StartsWith("<"))
            {
                var msg = XElement.Parse(displayMsg);
                displayMsg = msg.Value;

            }
        }
        if(displayMsg != null)
        {
            var result = JsonConvert.DeserializeObject<NavErrorBody>(displayMsg);
            if (result.error != null)
                if (!string.IsNullOrEmpty(result.error.message))
                    displayMsg = result.error.message;

            if (displayMsg.IndexOf("CorrelationId") >= 0)
                displayMsg = displayMsg.Remove(displayMsg.IndexOf("CorrelationId"));

            displayMsg = displayMsg.Replace("\n", " ").Replace(",", ";").Replace(System.Environment.NewLine, " ").Replace("'", " ").Replace("\"", " ");
        }

        return displayMsg;
    }
}

public class NavErrorBody
{
    public NavError error { get; set; }
}
public class NavError
{
    public string code { get; set; } = string.Empty;
    public string message { get; set; } = string.Empty;
}
