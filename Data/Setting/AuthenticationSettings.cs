using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Setting
{
    public class AuthenticationSettings
    {
        public string JWT_Secret { get; set; } = string.Empty;
        public string Client_URL { get; set; } = string.Empty;
        public string DocsLink { get; set; } = string.Empty;
        public string FrontEnd_URL { get; set; } = string.Empty;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public bool AllowSeeding { get; set; } = false;
    }
}
