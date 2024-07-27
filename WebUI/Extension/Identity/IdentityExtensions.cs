using System.Security.Claims;
using System.Security.Principal;

namespace Web.Extensions.Identity
{
    public static class IdentityExtensions
    {
        public static string UserId(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;

        public static string FullNames(this IPrincipal user) => 
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GivenName)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GivenName).Value;

        public static string UserCode(this IPrincipal user) => 
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.PrimarySid)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.PrimarySid).Value;

        public static string NavUser(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GroupSid)
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.GroupSid).Value;

        public static string UserActivity(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst("Activity")
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst("Activity").Value;

        public static string UserBranch(this IPrincipal user) =>
            ((ClaimsIdentity)user.Identity).FindFirst("Branch")
            == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst("Branch").Value;
        public static string UserCentre(this IPrincipal user) =>
           ((ClaimsIdentity)user.Identity).FindFirst("ResposibilityCentre")
           == null ? string.Empty : ((ClaimsIdentity)user.Identity).FindFirst("ResposibilityCentre").Value;

    }
}
