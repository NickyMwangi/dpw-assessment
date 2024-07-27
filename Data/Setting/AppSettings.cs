using Data.Interfaces;
using Data.Setting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Common.Setting;

public class AppSettings: IAppSettings
{
    public AppSettings(IConfiguration config)
    {
        Config = config;
        CorsOriginAllowed = Config.GetValue<string>("corsOriginsAllowed");
        Config.Bind("AuthenticationSettings", AuthenticationSettings);
    }

    private IConfiguration Config { get; }
    public string CorsOriginAllowed { get; } = string.Empty;
    public AuthenticationSettings AuthenticationSettings { get; } = new();
}
