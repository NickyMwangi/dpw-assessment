using Data.Setting;
using Library.Common.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAppSettings
    {
        string CorsOriginAllowed { get; }
        AuthenticationSettings AuthenticationSettings { get; }
    }
}
