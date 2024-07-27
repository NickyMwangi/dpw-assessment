using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public string FullNames { get; set; } = string.Empty;

        public string UserCode { get; set; } = string.Empty;
    }
}
