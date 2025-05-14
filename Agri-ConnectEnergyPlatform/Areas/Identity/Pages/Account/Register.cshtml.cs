using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Agri_ConnectEnergyPlatform.Areas.Identity.Data;

public class RegisterModel : PageModel
{
    private readonly UserManager<Agri_ConnectEnergyPlatformUser> _userManager;
    private readonly SignInManager<Agri_ConnectEnergyPlatformUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterModel(
        UserManager<Agri_ConnectEnergyPlatformUser> userManager,
        SignInManager<Agri_ConnectEnergyPlatformUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var user = new Agri_ConnectEnergyPlatformUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Ensure the "Farmer" role exists
                if (!await _roleManager.RoleExistsAsync("Farmer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                }

                // Assign Farmer role
                await _userManager.AddToRoleAsync(user, "Farmer");

                // Log them in
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}
