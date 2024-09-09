using GameCritic.DTOModels;
using GameCriticBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenresController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;

        public GenresController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllGenres")]
        public ActionResult<IEnumerable<GenreDto>> GetAllGenres()
        {
            try
            {
                var result = _context.Genres;
                var mappedResult = result.Select(x =>
                    new GenreDto
                    {
                        Idgenre = x.Idgenre,
                        GenreName = x.GenreName,
                        Description = x.Description,
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GameDto> GetGenreById(int id)
        {
            try
            {
                var result =
                    _context.Genres
                        .FirstOrDefault(x => x.Idgenre == id);

                if (result == null)
                {
                    return NotFound($"Could not find genre with id {id}");
                }

                var mappedResult = new GenreDto
                {
                    Idgenre = result.Idgenre,
                    GenreName = result.GenreName,
                    Description = result.Description,
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("CreateGenre")]
        public ActionResult<GenreDto> PostGenre([FromBody] GenreDto genreDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newGenre = new Genre
                {
                    Idgenre = genreDto.Idgenre,
                    GenreName = genreDto.GenreName,
                    Description= genreDto.Description,
                };

                _context.Genres.Add(newGenre);

                _context.SaveChanges();

                genreDto.Idgenre = newGenre.Idgenre;

                return Ok(genreDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateGenre")]
        public ActionResult<GenreDto> PutGenre(int id, [FromBody] GenreDto updatedGenreDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedGenre = _context.Genres.FirstOrDefault(x => x.Idgenre == id);
                if (updatedGenre == null)
                {
                    return NotFound();
                }

                updatedGenre.GenreName = updatedGenreDto.GenreName;
                updatedGenre.Description = updatedGenreDto.Description;

                _context.SaveChanges();

                return Ok(updatedGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteGenre")]
        public ActionResult<GenreDto> DeleteGenre(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var genreToDelete = _context.Genres.FirstOrDefault(x => x.Idgenre == id);
                if (genreToDelete == null)
                {
                    return NotFound();
                }

                var gameGenresWithGenre = _context.GameGenres.Where(x => x.GenreId == id).ToList();
                foreach (var gameGenre in gameGenresWithGenre)
                {
                    _context.GameGenres.Remove(gameGenre);
                }

                _context.Genres.Remove(genreToDelete);

                _context.SaveChanges();

                return Ok(genreToDelete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
