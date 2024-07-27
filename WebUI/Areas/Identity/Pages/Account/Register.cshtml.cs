using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Extensions;
using Data.Interfaces;
using Data.Entities;
using Library.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using WebUI.Extensions.Helpers;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;
        private readonly IRepoService _repo;
        private readonly IIdentityService _idService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger, IEmailService emailService,
            IRepoService repo, IIdentityService idService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _repo = repo;
            _idService = idService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public List<SelectListItem> UserTypeList { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "User Type")]
            public string UserType { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            UserTypeList = new List<SelectListItem>()
                    {
                        new SelectListItem{Text="Client", Value="Client"},
                        new SelectListItem{Text="Employee", Value="Employee"}
                    };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.UserType == "Client")
                        await _idService.AddUserRoleAsync(user.Id, "Client");
                    else await _idService.AddUserRoleAsync(user.Id, "Resource");
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code },
                        protocol: Request.Scheme);

                    await _emailService.SendEmailConfirmationAsync(user.FullNames, Input.Email, callbackUrl);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        ViewData["alert"] = AlertEnum.info.Swal_Message("Please check your email to confirm your account.", "Email Sent");
                        // return LocalRedirect(returnUrl);
                        return RedirectToPage("Login");
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        ViewData["alert"] = AlertEnum.success.Swal_Message("Registration Successfull");
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //ViewData["alert"] = AlertEnum.error.Swal_Message(ModelState.AlertMessage(false));
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
