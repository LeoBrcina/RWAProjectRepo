using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GameCriticWebApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using GameCriticBL.Models;
using GameCriticWebApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Client;

namespace GameCriticWebApp.Controllers
{
    public class UserGamerController : Controller
    {
        private readonly RwaprojectDbContext _context;
        public UserGamerController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var userGamerVMs = _context.UserGamers
                    .Include(x => x.Reviews)
                    .Select(x => new UserGamerVM
                    {
                        IduserGamer = x.IduserGamer,
                        Username = x.Username,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        City = x.City,
                        PostalCode = x.PostalCode,
                        HomeAddress = x.HomeAddress,
                        RoleName = x.UserRole.RoleName
                    })
                    .ToList();

                return View(userGamerVMs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Login(string returnUrl)
        {
            var loginVM = new LoginVM
            {
                ReturnUrl = returnUrl
            };

            return View(loginVM);
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            var existingUser =
                _context
                    .UserGamers
                    .Include(x => x.UserRole)
                    .FirstOrDefault(x => x.Username == loginVM.Username);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            var b64hash = PasswordHashProvider.GetHash(loginVM.Password, existingUser.PwdSalt);
            if (b64hash != existingUser.PwdHash)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, loginVM.Username),
                new Claim(ClaimTypes.Role, existingUser.UserRole.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (loginVM.ReturnUrl != null)
                return LocalRedirect(loginVM.ReturnUrl);
            else if (existingUser.UserRole.RoleName == "Admin")
                return RedirectToAction("Index", "Game");
            else if (existingUser.UserRole.RoleName == "User")
                return RedirectToAction("Search", "Game");
            else
                return View();
        }

        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserGamerVM userGamerVM)
        {
            var trimmedUsername = userGamerVM.Username.Trim();
            if (_context.UserGamers.Any(x => x.Username.Equals(trimmedUsername)))
            {
                ModelState.AddModelError("", "This username already exists!");
                return View();
            }

            var userRole = _context.UserRoles.FirstOrDefault(x => x.RoleName == "User");

            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(userGamerVM.Password, b64salt);

            var user = new UserGamer
            {
                IduserGamer = userGamerVM.IduserGamer,
                Username = userGamerVM.Username,
                PwdHash = b64hash,
                PwdSalt = b64salt,
                FirstName = userGamerVM.FirstName,
                LastName = userGamerVM.LastName,
                Email = userGamerVM.Email,
                City = userGamerVM.City,
                PostalCode = userGamerVM.PostalCode,
                HomeAddress = userGamerVM.HomeAddress,
                UserRoleId = 2,
            };

            _context.Add(user);

            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        public ActionResult Details(int id)
        {
            try
            {
                var userGamer = _context.UserGamers.Include(x => x.Reviews).Include(x => x.UserRole).FirstOrDefault(x => x.IduserGamer == id);
                var userGamerVM = new UserGamerVM
                {
                    IduserGamer = userGamer.IduserGamer,
                    Username = userGamer.Username,
                    FirstName = userGamer.FirstName,
                    LastName = userGamer.LastName,
                    Email = userGamer.Email,
                    City = userGamer.City,
                    PostalCode = userGamer.PostalCode,
                    HomeAddress = userGamer.HomeAddress,
                    RoleName = userGamer.UserRole.RoleName
                };

                return View(userGamerVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IActionResult ProfileDetails()
        {
            var username = HttpContext.User.Identity.Name;

            var userGamerDb = _context.UserGamers.First(x => x.Username == username);
            var userGamerVm = new UserGamerVM
            {
                IduserGamer = userGamerDb.IduserGamer,
                Username = userGamerDb.Username,
                FirstName = userGamerDb.FirstName,
                LastName = userGamerDb.LastName,
                Email = userGamerDb.Email,
                City = userGamerDb.City,
                PostalCode = userGamerDb.PostalCode,
                HomeAddress = userGamerDb.HomeAddress,
            };

            return View(userGamerVm);
        }

        [Authorize]
        public IActionResult ProfileEdit(int id)
        {
            var userGamerDb = _context.UserGamers.First(x => x.IduserGamer == id);
            var userGamerVm = new UserGamerVM
            {
                IduserGamer = userGamerDb.IduserGamer,
                Username = userGamerDb.Username,
                FirstName = userGamerDb.FirstName,
                LastName = userGamerDb.LastName,
                Email = userGamerDb.Email,
                City = userGamerDb.City,
                PostalCode = userGamerDb.PostalCode,
                HomeAddress = userGamerDb.HomeAddress,
            };

            return View(userGamerVm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProfileEdit(int id, UserGamerVM userGamerVm)
        {
            var userGamerDb = _context.UserGamers.First(x => x.IduserGamer == id);
            userGamerDb.FirstName = userGamerVm.FirstName;
            userGamerDb.LastName = userGamerVm.LastName;
            userGamerDb.Email = userGamerVm.Email;
            userGamerDb.City = userGamerVm.City;
            userGamerDb.PostalCode = userGamerVm.PostalCode;
            userGamerDb.HomeAddress = userGamerVm.HomeAddress;

            _context.SaveChanges();

            return RedirectToAction("ProfileDetails");
        }

        public JsonResult GetProfileData(int id)
        {
            var userGamerDb = _context.UserGamers.First(x => x.IduserGamer == id);
            return Json(new
            {
                userGamerDb.FirstName,
                userGamerDb.LastName,
                userGamerDb.Email,
                userGamerDb.City,
                userGamerDb.PostalCode,
                userGamerDb.HomeAddress,
            });
        }

        [Authorize]
        public IActionResult ProfileDetailsPartial()
        {
            var username = HttpContext.User.Identity.Name;

            var userGamerDb = _context.UserGamers.First(x => x.Username == username);
            var userGamerVm = new UserGamerVM
            {
                IduserGamer = userGamerDb.IduserGamer,
                Username = userGamerDb.Username,
                FirstName = userGamerDb.FirstName,
                LastName = userGamerDb.LastName,
                Email = userGamerDb.Email,
                City = userGamerDb.City,
                PostalCode = userGamerDb.PostalCode,
                HomeAddress = userGamerDb.HomeAddress,
            };

            return PartialView("_ProfileDetailsPartial", userGamerVm);
        }

        [HttpPut]
        public ActionResult SetProfileData(int id, [FromBody] UserGamerVM userGamerVm)
        {
            var userGamerDb = _context.UserGamers.First(x => x.IduserGamer == id);
            userGamerDb.FirstName = userGamerVm.FirstName;
            userGamerDb.LastName = userGamerVm.LastName;
            userGamerDb.Email = userGamerVm.Email;
            userGamerDb.City = userGamerVm.City;
            userGamerDb.PostalCode = userGamerVm.PostalCode;
            userGamerDb.HomeAddress = userGamerVm.HomeAddress;

            _context.SaveChanges();

            return Ok();
        }
    }
    }

