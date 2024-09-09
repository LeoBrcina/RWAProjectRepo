using GameCriticBL.Models;
using GameCriticWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace GameCriticWebApp.Controllers
{
    public class GenreController : Controller
    {
        private readonly RwaprojectDbContext _context;
        public GenreController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                var genreVMs = _context.Genres.Select(x => new GenreVM
                {
                    Idgenre = x.Idgenre,
                    GenreName = x.GenreName,
                    Description = x.Description,
                }).ToList();

                return View(genreVMs);
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
                var genre = _context.Genres.FirstOrDefault(x => x.Idgenre == id);
                var genreVM = new GenreVM
                {
                    Idgenre = genre.Idgenre,
                    GenreName = genre.GenreName,
                    Description = genre.Description,
                };

                return View(genreVM);
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
        public ActionResult Create(GenreVM genre)
        {
            try
            {
                if (_context.Genres.Any(x => x.GenreName == genre.GenreName))
                {
                    ModelState.AddModelError("", "Genre with the same name already exists!");
                    return View();
                }

                var newGenre = new Genre
                {
                    GenreName = genre.GenreName,
                    Description = genre.Description,
                };

                _context.Genres.Add(newGenre);

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
                var genre = _context.Genres.FirstOrDefault(x => x.Idgenre == id);
                var genreVM = new GenreVM
                {
                    Idgenre = genre.Idgenre,
                    GenreName = genre.GenreName,
                    Description = genre.Description,
                };

                return View(genreVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GenreVM genre)
        {
            try
            {
                if (_context.Genres.Any(x => x.GenreName == genre.GenreName && x.Idgenre == genre.Idgenre))
                {
                    var existingGenre = _context.Genres.FirstOrDefault(x => x.GenreName == genre.GenreName);
                    if (existingGenre != null && existingGenre.Description != genre.Description)
                    {
                        ModelState.AddModelError("", "Genre with the same name already exists!");
                        return View();
                    }
                }

                var dbGenre = _context.Genres.FirstOrDefault(x => x.Idgenre == id);
                dbGenre.GenreName = genre.GenreName;
                dbGenre.Description = genre.Description;

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
                var genre = _context.Genres.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgenre == id);
                var genreVM = new GenreVM
                {
                    Idgenre = genre.Idgenre,
                    GenreName = genre.GenreName,
                    Description = genre.Description,
                };

                return View(genreVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GenreVM genre)
        {
            try
            {
                var dbGenreToDelete = _context.Genres.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgenre == id);

                _context.GameGenres.RemoveRange(dbGenreToDelete.GameGenres);

                _context.Genres.Remove(dbGenreToDelete);

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
