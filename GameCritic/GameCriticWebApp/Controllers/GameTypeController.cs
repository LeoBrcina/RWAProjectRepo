using GameCriticBL.Models;
using GameCriticWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCriticWebApp.Controllers
{
    public class GameTypeController : Controller
    {
        private readonly RwaprojectDbContext _context;
        public GameTypeController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var gameTypeVms = _context.GameTypes.Select(x => new GameTypeVM
                {
                    IdgameType = x.IdgameType,
                    GameTypeName = x.GameTypeName,
                    Description = x.Description,
                }).ToList();

                return View(gameTypeVms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                var gameType = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                var gameTypeVM = new GameTypeVM
                {
                    IdgameType = gameType.IdgameType,
                    GameTypeName = gameType.GameTypeName,
                    Description = gameType.Description,
                };

                return View(gameTypeVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GameTypeVM gameType)
        {
            try
            {
                if (_context.GameTypes.Any(x => x.GameTypeName == gameType.GameTypeName))
                {
                    ModelState.AddModelError("", "Game type with the same name already exists!");
                    return View();
                }

                var newGameType = new GameType
                {
                    GameTypeName = gameType.GameTypeName,
                    Description= gameType.Description,
                };

                _context.GameTypes.Add(newGameType);

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
            try
            {
                var gameType = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                var gameTypeVM = new GameTypeVM
                {
                    IdgameType = gameType.IdgameType,
                    GameTypeName = gameType.GameTypeName,
                    Description = gameType.Description,
                };

                return View(gameTypeVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GameTypeVM gameType)
        {
            try
            {
                if (_context.GameTypes.Any(x => x.GameTypeName == gameType.GameTypeName && x.IdgameType == gameType.IdgameType))
                {
                    var existingGameType = _context.GameTypes.FirstOrDefault(x => x.GameTypeName == gameType.GameTypeName);
                    if (existingGameType != null && existingGameType.Description != gameType.Description)
                    {
                        ModelState.AddModelError("", "Game type with the same name already exists!");
                        return View();
                    }
                }

                var dbGameType = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                dbGameType.GameTypeName = gameType.GameTypeName;
                dbGameType.Description = gameType.Description;

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
                var gameType = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);
                var gameTypeVM = new GameTypeVM
                {
                    IdgameType = gameType.IdgameType,
                    GameTypeName = gameType.GameTypeName,
                    Description = gameType.Description,
                };

                return View(gameTypeVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GameTypeVM gameType)
        {
            try
            {
                var dbGameTypeToDelete = _context.GameTypes.FirstOrDefault(x => x.IdgameType == id);

                var gamesWithGameType = _context.Games.Where(x => x.GameTypeId == id).ToList();

                foreach (var game in gamesWithGameType)
                {
                    var reviewsWithGame = _context.Reviews.Where(x => x.GameId == game.Idgame).ToList();
                    foreach (var review in reviewsWithGame)
                    {
                        _context.Reviews.Remove(review);
                    }
                    _context.Games.Remove(game);
                }

                _context.GameTypes.Remove(dbGameTypeToDelete);

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
