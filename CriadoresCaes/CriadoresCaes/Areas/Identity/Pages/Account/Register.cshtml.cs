using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CriadoresCaes.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using CriadoresCaes.Models;

namespace CriadoresCaes.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }


        /// <summary>
        /// Model usado para 'transportar' os dados para a interface de 'Registar'
        /// </summary>
        [BindProperty]//garante a existência de 'memória' entre o Model e a interface
        public InputModel Input { get; set; }

        /// <summary>
        /// serve para redireccionar o utilizador para o 'local' de origem
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /*
         * Classe usada para 'transportar/recolher' os dados da Página para dentro do 'código'
         */
        public class InputModel
        {
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
            [Compare("Password", ErrorMessage = "A password e a sua confirmação não correspondem.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Ao anexar um objeto deste tipo ao 'InputModel' estamos a 
            /// permitir a recolha dos dados do Criador
            /// </summary>
            public Criadores Criador { get; set; }

        }
        /// <summary>
        /// método a ser executado pela a página, quando o HTTP é invocado em GET
        /// </summary>
        /// <param name="returnUrl">link para redirecionar o utilizador, se fornecido</param>
        /// <returns></returns>
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            //lista dos 'providers' para efetuar autenticaç~~ao externa
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// método a ser executado pela a página, quando o HTTP é invocado em POST
        /// </summary>
        /// <param name="returnUrl">link para redirecionar o utilizador, se fornecido</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
