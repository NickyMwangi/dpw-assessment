using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.seed
{
    //MUST be included in the project with JSON data
    public static class JsonInterpreter
    {
        public static string GetResourceAsString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            if (resourceName == null) throw new Exception("Filename cannot be null");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new Exception("Stream cannot be null");
                using (var reader = new StreamReader(stream))
                {
                    var strJson = reader.ReadToEnd();
                    return strJson;
                }
            }
        }

        public static string GetResourceName(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
            if (resourceName == null) throw new Exception("Filename cannot be null");
            else return resourceName;
        }
    }
}
