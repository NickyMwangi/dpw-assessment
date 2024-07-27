using System;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Services;
using Data.Entities;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Extensions.Helpers;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepoService _repo;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager, IRepoService repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        [ViewData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                //return NotFound($"Unable to load user with ID '{userId}'.");
                StatusMessage = $"Unable to load user with ID '{userId}'.";
                ViewData["alert"] = AlertEnum.error.Swal_Message(StatusMessage);
            }
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
                
                ViewData["alert"] = AlertEnum.info.Swal_Message(StatusMessage);
            }
            catch (CustomException ex)
            {
                StatusMessage = $"Error confirming your email. {ex.Message}";
                ViewData["alert"] = AlertEnum.error.Swal_Message(StatusMessage);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error confirming your email. {ex.Message}";
                ViewData["alert"] = AlertEnum.error.Swal_Message(StatusMessage);
            }
            
            return RedirectToPage("/Index");
            //return Page();
        }
    }
}
