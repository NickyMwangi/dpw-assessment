using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> IsRoleExistsAsync(string roleName);
        Task<IdentityRole> GetRoleAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(string roleName);
        Task<IList<Claim>> GetRoleClaimsAsync(string roleId);
        Task<IdentityResult> CreateRoleClaimAsync(string roleName, string claimType, string claimValue);
        Task<IdentityResult> AddUserRoleAsync(string userId, string roleName);
        Task<IList<string>> GetUserRolesAsync(string userId);
    }
}
