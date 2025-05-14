using Agri_ConnectEnergyPlatform.Areas.Identity.Data;
using Agri_ConnectEnergyPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Agri_ConnectEnergyPlatform.Controllers
{
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Agri_ConnectEnergyPlatformUser> _userManager;

        public FarmerController(ApplicationDbContext context, UserManager<Agri_ConnectEnergyPlatformUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult AddProduct() => View();

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var identityUserId = _userManager.GetUserId(User);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.IdentityUserId == identityUserId);

                if (farmer == null)
                {
                    return Unauthorized();
                }

                product.FarmerID = farmer.FarmerID;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("FarmerProducts");
            }
            return View(product);
        }

        public async Task<IActionResult> FarmerProducts()
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Employee"))
            {
                // Employee sees all
                var allProducts = await _context.Products.Include(p => p.Farmer).ToListAsync();
                return View(allProducts);
            }

            if (user != null)
            {
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);
                if (farmer != null)
                {
                    var products = await _context.Products
                        .Where(p => p.FarmerID == farmer.FarmerID)
                        .Include(p => p.Farmer)
                        .ToListAsync();

                    return View(products);
                }
            }

            return RedirectToAction("Index", "Home"); 
        }

    }
}
