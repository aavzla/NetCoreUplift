using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Identity.Pages.Account
{
    public class EditModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required, Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            public string UserId { get; set; }
            public string UserName { get; set; }
            public string UserRole { get; set; }

            //Add To modify properties of ApplicationUser Model
            [Required]
            public string Name { get; set; }
            [Required, Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required, Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

        }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            ReturnUrl = Url.Action("Index", "User", new { area = "Admin" });

            var user = (ApplicationUser)_userManager.FindByIdAsync(id).Result;
            Input.UserId = user.Id;
            Input.UserName = user.UserName;
            Input.Name = user.Name;
            Input.Email = user.Email;
            Input.PhoneNumber = user.PhoneNumber;
            Input.StreetAddress = user.StreetAddress;
            Input.City = user.City;
            Input.State = user.State;
            Input.PostalCode = user.PostalCode;

            IList<string> userRoles = _userManager.GetRolesAsync(user).Result;

            if (userRoles != null && userRoles.Any())
            {
                Input.UserRole = userRoles.First();
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "User", new { area = "Admin" });

            if (ModelState.IsValid)
            {
                var userFromDB = (ApplicationUser)_userManager.FindByIdAsync(Input.UserId).Result;
                if (Input.Email != userFromDB.Email)
                {
                    userFromDB.UserName = Input.Email;
                    _userManager.UpdateNormalizedUserNameAsync(userFromDB).Wait();
                    userFromDB.Email = Input.Email;
                    _userManager.UpdateNormalizedEmailAsync(userFromDB).Wait();
                }

                userFromDB.Name = Input.Name;
                userFromDB.PhoneNumber = Input.PhoneNumber;
                userFromDB.StreetAddress = Input.StreetAddress;
                userFromDB.City = Input.City;
                userFromDB.State = Input.State;
                userFromDB.PostalCode = Input.PostalCode;

                string role = Request.Form["rdUserRole"].ToString();

                if (role == StaticDetails.Admin)
                {
                    await _userManager.AddToRoleAsync(userFromDB, StaticDetails.Admin);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userFromDB, StaticDetails.Admin);
                }

                if (role == StaticDetails.Manager)
                {
                    await _userManager.AddToRoleAsync(userFromDB, StaticDetails.Manager);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(userFromDB, StaticDetails.Manager);
                }

                var passwordHash = _userManager.PasswordHasher.HashPassword(userFromDB, Input.Password);
                if (_userManager.PasswordHasher.VerifyHashedPassword(userFromDB, passwordHash, Input.Password) == PasswordVerificationResult.Success)
                {
                    userFromDB.PasswordHash = passwordHash;
                }
                else
                {
                    //This should not happen.
                    return Page();
                }

                var result = await _userManager.UpdateAsync(userFromDB);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User modified. Id = {userFromDB.Id} by Admin {User.Identity.Name}.");

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // The ModelState is not valid, redisplay form
            return Page();
        }
    }
}
