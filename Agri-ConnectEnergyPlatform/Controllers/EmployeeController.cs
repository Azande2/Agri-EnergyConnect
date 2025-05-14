using Agri_ConnectEnergyPlatform.Areas.Identity.Data;
using Agri_ConnectEnergyPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Agri_ConnectEnergyPlatform.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Agri_ConnectEnergyPlatformUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(ApplicationDbContext context, UserManager<Agri_ConnectEnergyPlatformUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult AddFarmer() => View();

        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                var identityUser = new Agri_ConnectEnergyPlatformUser
                {
                    UserName = farmer.Email,
                    Email = farmer.Email
                };

                var result = await _userManager.CreateAsync(identityUser, farmer.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Farmer"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                    }

                    await _userManager.AddToRoleAsync(identityUser, "Farmer");

                    // Link IdentityUserId to Farmer table
                    farmer.IdentityUserId = identityUser.Id;
                    _context.Farmers.Add(farmer);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Farmer '{farmer.FirstName} {farmer.LastName}' created. Login with Email: {farmer.Email} and Password: {farmer.Password}.";
                    return RedirectToAction("FarmerList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(farmer);
        }

        public async Task<IActionResult> FarmerList()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

        public async Task<IActionResult> Market()
        {
            // Includes the related Farmer info for each product
            var products = await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> FilterMarket(string category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Products.Include(p => p.Farmer).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);
            if (startDate.HasValue)
                query = query.Where(p => p.ProductionDate >= startDate);
            if (endDate.HasValue)
                query = query.Where(p => p.ProductionDate <= endDate);

            var filtered = await query.ToListAsync();
            return View("Market", filtered);
        }
    }
}
