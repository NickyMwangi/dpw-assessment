using Data.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public class IdentityClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly IRepoService repo;
        public IdentityClaimsFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options, IRepoService _repo)
            : base(userManager, roleManager, options)
        {
            repo = _repo;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            //identity.AddClaims(new[] {
            //    new Claim(ClaimTypes.PrimarySid, applicant.Profile_No??""),
            //    new Claim(ClaimTypes.GroupSid, applicant.National_ID_No??""),
            //});
            return identity;
        }

    }
}
