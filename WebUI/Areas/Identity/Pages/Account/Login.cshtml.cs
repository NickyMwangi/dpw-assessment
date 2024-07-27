using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Data.Entities;
using Data.Interfaces;
using Library.Utility;
using WebUI.Extensions.Helpers;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailService _emailService;

        private readonly IRepoService _repo;
        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IRepoService repo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _repo = repo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [ViewData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; } = String.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = String.Empty;

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            public string PopupMessage { get; set; } = String.Empty;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                Response.Cookies.Delete("DPWAssessment");
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                returnUrl ??= Url.Content("~/");

                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            }
            catch (System.Exception ex)
            {
                ViewData["Alert"] = AlertEnum.error.Swal_Message(ex.AlertMessage());
            }

            ReturnUrl = returnUrl;

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            try
            {

                if (ModelState.IsValid)
                {

                    var user = await _userManager.FindByNameAsync(Input.UserName);
                    await _signInManager.SignInAsync(user, true);

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, user.PhoneNumber, Input.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                        if (_signInManager.IsSignedIn(User))
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ViewData["alert"] = AlertEnum.error.Swal_Message("Invalid login attempt.", null, true);
                        return Page();
                    }

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid model.");
                }
            }
            catch (System.Exception ex)
            {
                ViewData["Alert"] = AlertEnum.error.Swal_Message(ex.AlertMessage());
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
