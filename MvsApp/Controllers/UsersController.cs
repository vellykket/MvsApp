using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsApplication.Models;
using MvsApplication.ViewModels;

namespace MvsApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly MvsAppContext _context;
        public UsersController(MvsAppContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
             User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
             _context.Users.Add(new User
                 {
                     Email = model.Email,
                     Firstname = model.Firstname,
                     Lastname = model.Lastname,
                     Password = model.Password,
                     Accounts = new List<Account>() {new Account
                     {
                      BalanceName = model.BalanceName,
                      BalanceNumber = RandomString(),
                      Balance = 0,
                     }}
                 });
                 await _context.SaveChangesAsync();
                 await Authenticate(model.Email);
                 return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email == model.Email && u.Password == model.Password
                );
                if (user != null)
                {
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }
        
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        
        private async Task Authenticate(string userEmail)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userEmail)
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        private string RandomString()
        {
            var balanceNumber = "";
            Random rnd = new Random();
            for (int i = 0; i < 16; i++)
            {
                balanceNumber = balanceNumber + rnd.Next(0, 10);
            }
            return balanceNumber;
        }
        
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string email)
        {
            if (_context.Users.Any(e => e.Email == email))
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }
    }
}