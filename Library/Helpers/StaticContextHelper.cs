using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Helpers
{
    public class StaticContextHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static IConfigurationBuilder Getbuilder()
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");
            return builder;
        }
        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static string GetAppSetting(string key)
        {
            //return Convert.ToString(ConfigurationManager.AppSettings[key]);
            var builder = Getbuilder();
            var GetAppStringData = builder.Build().GetValue<string>(key);
            return GetAppStringData;
        }


    }
}
