using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class ApplicationRole : IdentityRole
    {
        private const string CacheKey = "4BA44A3F-F728-42D8-9387-7577EDC0DD99_Role_Menu_";

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public static string GetCacheKey(string roleName)
        {
            return CacheKey + roleName;
        }
    }
}
