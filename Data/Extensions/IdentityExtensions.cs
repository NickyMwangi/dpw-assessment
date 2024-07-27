using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class IdentityExtensions
    {
        public static string UserId(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

        public static string EmailAddress(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Email).Value;

        public static string NationalID(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GroupSid)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GroupSid).Value;
    }
}
