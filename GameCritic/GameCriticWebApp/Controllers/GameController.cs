using GameCriticBL.Models;
using GameCriticWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging;
using NuGet.Protocol;
using System.Diagnostics.CodeAnalysis;

namespace GameCriticWebApp.Controllers
{
    public class GameController : Controller
    {
        private readonly RwaprojectDbContext _context;
        private readonly IConfiguration _configuration;
        public GameController(RwaprojectDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private List<SelectListItem> GetGameTypeListItems()
        {
            var gameTypeListItemsJson = HttpContext.Session.GetString("GameTypeListItems");

            List<SelectListItem> gameTypeListItems;
            if (gameTypeListItemsJson == null)
            {
                gameTypeListItems = _context.GameTypes
                    .Select(x => new SelectListItem
                    {
                        Text = x.GameTypeName,
                        Value = x.IdgameType.ToString()
                    }).ToList();

                HttpContext.Session.SetString("GameTypeListItems", gameTypeListItems.ToJson());
            }
            else
            {
                gameTypeListItems = gameTypeListItemsJson.FromJson<List<SelectListItem>>();
            }

            return gameTypeListItems;
        }

        private List<SelectListItem> GetGenreListItems()
        {
            return _context.Genres
                .Select(x => new SelectListItem
                {
                    Text = x.GenreName,
                    Value = x.Idgenre.ToString()
                }).ToList();
        }

        public ActionResult Search(SearchVM searchVm)
        {
            try
            {
                if (string.IsNullOrEmpty(searchVm.Q) && string.IsNullOrEmpty(searchVm.Submit))
                {
                    searchVm.Q = Request.Cookies["query"];
                }

                IQueryable<Game> games = _context.Games
                    .Include(x => x.GameType);

                if (!string.IsNullOrEmpty(searchVm.Q))
                {
                    games = games.Where(x => x.GameName.Contains(searchVm.Q));
                }

                var filteredCount = games.Count();

                switch (searchVm.OrderBy.ToLower())
                {
                    case "idgame":
                        games = games.OrderBy(x => x.Idgame);
                        break;
                    case "gamename":
                        games = games.OrderBy(x => x.GameName);
                        break;
                    case "gametype":
                        games = games.OrderBy(x => x.GameType.GameTypeName);
                        break;
                    case "size":
                        games = games.OrderBy(x => x.Size);
                        break;
                }

                games = games.Skip((searchVm.Page - 1) * searchVm.Size).Take(searchVm.Size); 

                searchVm.Games =
                    games.Select(x => new GameVM
                    {
                        Idgame = x.Idgame,
                        GameName = x.GameName,
                        Description = x.Description,
                        Size = x.Size,
                        GameTypeId = x.GameTypeId,
                        GameTypeName = x.GameType.GameTypeName
                    })
                    .ToList();

                var expandPages = _configuration.GetValue<int>("Paging:ExpandPages");
                searchVm.LastPage = (int)Math.Ceiling(1.0 * filteredCount / searchVm.Size);
                searchVm.FromPager = searchVm.Page > expandPages ?
                    searchVm.Page - expandPages :
                    1;
                searchVm.ToPager = (searchVm.Page + expandPages) < searchVm.LastPage ?
                    searchVm.Page + expandPages :
                    searchVm.LastPage;

                var option = new CookieOptions { Expires = DateTime.Now.AddMinutes(15) };
                Response.Cookies.Append("query", searchVm.Q ?? "", option);

                return View(searchVm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var gameVms = _context.Games
                    .Include(x => x.GameType)
                    .Include(x => x.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                    .Select(x => new GameVM
                    {
                        Idgame = x.Idgame,
                        GameName = x.GameName,
                        GameTypeId = x.GameTypeId,
                        Description = x.Description,
                        Size = x.Size,
                        GameTypeName = x.GameType.GameTypeName,
                        GenreNames = x.GameGenres.Select(gg => gg.Genre.GenreName).ToList()
                    })
                    .ToList();

                ViewBag.GenreDdlItems = GetGenreListItems();

                return View(gameVms);
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
                var game = _context.Games.Include(x => x.GameType).Include(x => x.GameGenres).ThenInclude(gg => gg.Genre).FirstOrDefault(x => x.Idgame == id);
                var gameVM = new GameVM
                {
                    Idgame = game.Idgame,
                    GameName = game.GameName,
                    GameTypeId = game.GameTypeId,
                    Description= game.Description,
                    Size= game.Size,
                    GameTypeName = game.GameType.GameTypeName,
                    GenreNames = game.GameGenres.Select(gg => gg.Genre.GenreName).ToList()
                };

                ViewBag.GenreDdlItems = GetGenreListItems();

                return View(gameVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.GameTypeDdlItems = GetGameTypeListItems();
                ViewBag.GenreDdlItems = GetGenreListItems();

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GameVM game)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.GameTypeDdlItems = GetGameTypeListItems();
                    ViewBag.GenreDdlItems = GetGenreListItems();

                    ModelState.AddModelError("", "Failed to create the game!");

                    return View();
                }

                if (_context.Games.Any(x => x.GameName == game.GameName))
                {
                    ModelState.AddModelError("", "Game with the same name already exists!");
                    return View();
                }

                var newGame = new Game
                {
                    GameName = game.GameName,
                    GameTypeId = game.GameTypeId,
                    Description = game.Description,
                    Size = game.Size,
                };

                _context.Games.Add(newGame);

                _context.SaveChanges();

                var gameGenres = game.GenreIds.Select(genreId => new GameGenre
                {
                    GameId = newGame.Idgame,
                    GenreId = genreId
                }).ToList();

                _context.GameGenres.AddRange(gameGenres);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.GameTypeDdlItems = GetGameTypeListItems();
            ViewBag.GenreDdlItems = GetGenreListItems();

            var game = _context.Games.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgame == id);
            var gameVM = new GameVM
            {
                Idgame = game.Idgame,
                GameName = game.GameName,
                GameTypeId = game.GameTypeId,
                Description = game.Description,
                Size = game.Size,
                GenreIds = game.GameGenres.Select(x => x.GenreId).ToList()
            };

            return View(gameVM);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GameVM game)
        {
            try
            {
                if (_context.Games.Any(x => x.GameName == game.GameName))
                {
                    var existingGame = _context.Games.FirstOrDefault(x => x.GameName == game.GameName);
                    if (existingGame != null && existingGame.Description != game.Description)
                    {
                        ModelState.AddModelError("", "Game with the same name already exists!");
                        return View();
                    }
                }

                var dbGame = _context.Games.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgame == id);
                dbGame.GameName = game.GameName;
                dbGame.GameTypeId = game.GameTypeId;
                dbGame.Description = game.Description;
                dbGame.Size = game.Size;

                _context.RemoveRange(dbGame.GameGenres);
                var gameGenres = game.GenreIds.Select(x => new GameGenre { GameId = id, GenreId = x });
                dbGame.GameGenres.AddRange(gameGenres);

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
                var game = _context.Games.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgame == id);
                var gameVM = new GameVM
                {
                    Idgame = game.Idgame,
                    GameName = game.GameName,
                    Description = game.Description,
                    Size = game.Size,
                    GenreIds = game.GameGenres.Select(x => x.GenreId).ToList()
                };

                return View(gameVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Game game)
        {
            try
            {
                var dbGameToDelete = _context.Games.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgame == id);

                _context.GameGenres.RemoveRange(dbGameToDelete.GameGenres);

                var reviewForGame = _context.Reviews.Where(x => x.GameId == id).ToList();

                foreach (var review in reviewForGame)
                {
                    _context.Reviews.Remove(review);
                }

                _context.Games.Remove(dbGameToDelete);

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
