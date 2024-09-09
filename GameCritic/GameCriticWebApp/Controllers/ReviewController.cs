using GameCriticBL.Models;
using GameCriticWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace GameCriticWebApp.Controllers
{
    public class ReviewController : Controller
    {
        private readonly RwaprojectDbContext _context;
        public ReviewController(RwaprojectDbContext context)
        {
            _context = context;
        }

        private List<SelectListItem> GetGameListItems()
        {
            var gameListItemsJson = HttpContext.Session.GetString("GameListItems");

            List<SelectListItem> gameListItems;
            if (gameListItemsJson == null)
            {
                gameListItems = _context.Games
                    .Select(x => new SelectListItem
                    {
                        Text = x.GameName,
                        Value = x.Idgame.ToString()
                    }).ToList();

                HttpContext.Session.SetString("GameListItems", gameListItems.ToJson());
            }
            else
            {
                gameListItems = gameListItemsJson.FromJson<List<SelectListItem>>();
            }

            return gameListItems;
        }

        private List<SelectListItem> GetUserGamerListItems()
        {
            var userGamerListItemsJson = HttpContext.Session.GetString("UserGamerListItems");

            List<SelectListItem> userGamerListItems;
            if (userGamerListItemsJson == null)
            {
                userGamerListItems = _context.UserGamers
                    .Select(x => new SelectListItem
                    {
                        Text = x.Username,
                        Value = x.IduserGamer.ToString()
                    }).ToList();

                HttpContext.Session.SetString("UserGamerListItems", userGamerListItems.ToJson());
            }
            else
            {
                userGamerListItems = userGamerListItemsJson.FromJson<List<SelectListItem>>();
            }

            return userGamerListItems;
        }

        public ActionResult Index()
        {
            try
            {

                var reviewVMs = _context.Reviews
                    .Include(x => x.Game)
                    .Include(x => x.Gamer)
                    .Select(x => new ReviewVM
                    {
                        Idreview = x.Idreview,
                        GameName = x.Game.GameName,
                        Rating = x.Rating,
                        ReviewText = x.ReviewText,
                        GameId = x.GameId,
                        GamerId = x.GamerId,
                        Username = x.Gamer.Username
                    })
                    .ToList();

                return View(reviewVMs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var username = HttpContext.User.Identity.Name;

                var user = _context.UserGamers.FirstOrDefault(x => x.Username == username);

                var review = _context.Reviews.Include(x => x.Game).Include(x => x.Gamer).FirstOrDefault(x => x.Idreview == id);
                var reviewVM = new ReviewVM
                {
                    Idreview =  review.Idreview,
                    GameId = review.GameId,
                    GamerId = user.IduserGamer,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    GameName = review.Game.GameName,
                };

                return View(reviewVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.GameDdlItems = GetGameListItems();

                ViewBag.UserGamerDdlItems = GetUserGamerListItems();

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReviewVM review)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.GameDdlItems = _context.Games
                    .Select(x => new SelectListItem
                    {
                        Text = x.GameName,
                        Value = x.Idgame.ToString()
                    });

                    ModelState.AddModelError("", "Failed to write the review!");

                    return View();
                }

                var username = HttpContext.User.Identity.Name;

                var user = _context.UserGamers.FirstOrDefault(x => x.Username == username);

                var newReview = new Review
                {
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    GameId = review.GameId,
                    GamerId = user.IduserGamer,
                };

                _context.Reviews.Add(newReview);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.GameDdlItems = GetGameListItems();

            ViewBag.UserGamerDdlItems = GetUserGamerListItems();

            var review = _context.Reviews.FirstOrDefault(x => x.Idreview == id);
            var reviewVM = new ReviewVM
            {
               Idreview = review.Idreview,
               Rating = review.Rating,
               ReviewText = review.ReviewText,
            };

            return View(reviewVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ReviewVM review)
        {
            try
            {
                var dbReview = _context.Reviews.FirstOrDefault(x => x.Idreview == id);
                dbReview.Rating = review.Rating;
                dbReview.ReviewText = review.ReviewText;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                var username = HttpContext.User.Identity.Name;

                var review = _context.Reviews.Include(x => x.Game).Include(x => x.Gamer).FirstOrDefault(x => x.Idreview == id);
                var reviewVM = new ReviewVM
                {
                    Idreview = review.Idreview,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText,
                    Username = username.ToString(),
                    GameName = review.Game.GameName
                };

                return View(reviewVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Review review)
        {
            try
            {
                var dbReview = _context.Reviews.FirstOrDefault(x => x.Idreview == id);

                _context.Reviews.Remove(dbReview);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
