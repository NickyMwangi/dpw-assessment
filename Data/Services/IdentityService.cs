using Data.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepoService repo;

        public IdentityService(UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager, IRepoService _repo)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            repo = _repo;
        }

        public Task<bool> IsRoleExistsAsync(string roleName)
        {
            return roleManager.RoleExistsAsync(roleName);
        }

        public Task<IdentityRole> GetRoleAsync(string roleName)
        {
            return roleManager.FindByNameAsync(roleName);
        }

        public Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            return roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role.Name != roleName)
                return await roleManager.SetRoleNameAsync(role, roleName);
            else
                return await roleManager.UpdateAsync(role);
        }

        public async Task<IList<Claim>> GetRoleClaimsAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
                return await roleManager.GetClaimsAsync(role);
            return new List<Claim>();
        }
        public async Task<IdentityResult> CreateRoleClaimAsync(string roleName, string claimType, string claimValue)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var roleClaim = new Claim(claimType, claimValue);
            var roleClaimList = await roleManager.GetClaimsAsync(role);
            IdentityResult result = null;
            if (!roleClaimList.Any(a => a.Type == claimType && a.Value == claimValue))
            {
                try
                {
                    result = await roleManager.AddClaimAsync(role, roleClaim);
                    if (!result.Succeeded)
                        return result;
                }
                catch (Exception ex)
                {
                    _ = ex.Message;
                }

            }
            return result;
        }

        public async Task<IdentityResult> AddUserRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);
                if (!await userManager.IsInRoleAsync(user, roleName))
                    return await userManager.AddToRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return null;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var userRoles = await userManager.GetRolesAsync(user);
            return userRoles;
        }

    }
}
