using GameCritic.DTOModels;
using GameCriticBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace GameCritic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly RwaprojectDbContext _context;
        public GamesController(RwaprojectDbContext context)
        {
            _context = context;
        }

        private void Log(int level, string message)
        {
            _context.LogItems.Add(
                new LogItem
                {
                    Tmstamp = DateTime.Now,
                    Lvl = level,
                    Txt = message
                });

            _context.SaveChanges();
        }

        [HttpGet("GetAllGames")]
        public ActionResult<IEnumerable<GameDto>> GetAllGames()
        {
            try
            {
                var result = _context.Games;
                var mappedResult = result.Select(x =>
                    new GameDto
                    {
                        Idgame = x.Idgame,
                        GameName = x.GameName,
                        GameTypeId = x.GameTypeId,
                        Description = x.Description,
                        Size = x.Size
                    });

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while retrieving all gameDTOs: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GameDto> GetGameById(int id)
        {
            try
            {
                var result =
                    _context.Games
                        .FirstOrDefault(x => x.Idgame == id);

                if (result == null)
                {
                    Log(0, $"Game with id {id} not found.");
                    return NotFound($"Could not find game with id {id}");
                }

                var mappedResult = new GameDto
                {
                    Idgame = result.Idgame,
                    GameName = result.GameName,
                    GameTypeId = result.GameTypeId,
                    Description = result.Description,
                    Size = result.Size
                };
                if (mappedResult == null)
                {
                    Log(0, $"Game {mappedResult.Idgame} not found.");
                    return NotFound($"Could not find game with id {id}");
                }

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while retrieving gameDTO {id}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("Search")]
        public ActionResult<IEnumerable<Game>> Search(string name, int page = 0, int count = 10)
        {
            try
            {
                IEnumerable<Game> results = _context.Games;

                if (!String.IsNullOrEmpty(name))
                {
                    results = results.Where(g => g.GameName.Contains(name, StringComparison.OrdinalIgnoreCase));
                }

                results = results.Skip(page * count).Take(count);

                Log(0, "Game/s successfully found");
                return Ok(results);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while searching for games: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPost("CreateGame")]
        public ActionResult<GameDto> PostGame([FromBody] GameDto gameDto)
        {
            try
            {
                int maxId;
                if (_context.Games.Any())
                {
                    maxId = _context.Games.Max(x => x.Idgame);
                }
                else
                {
                    maxId = 0;
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (_context.Games.Any(x => x.GameName == gameDto.GameName))
                {
                    Log(5, $"A game with a name that already exists could not be created.");
                    return BadRequest(500);
                }

                var newGame = new Game
                {
                    GameName = gameDto.GameName,
                    GameTypeId = gameDto.GameTypeId,
                    Description = gameDto.Description,
                    Size = gameDto.Size,
                };

                _context.Games.Add(newGame);

                Log(0, $"Game {maxId + 1} added.");

                _context.SaveChanges();

                gameDto.Idgame = newGame.Idgame;

                return Ok(gameDto);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while creating a new game: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpPut("UpdateGame")]
        public ActionResult<GameDto> PutGame(int id, [FromBody] GameDto updatedGameDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedGame = _context.Games.FirstOrDefault(x => x.Idgame == id);
                if (updatedGame == null)
                {
                    Log(0, $"Game {updatedGame.Idgame} not found.");
                    return NotFound($"Could not find game with id {id}");
                }

                if (_context.Games.Any(x => x.GameName == updatedGameDto.GameName && x.Idgame != id))
                {
                    Log(5, $"A game name could not be updated to a name that already exists.");
                    return BadRequest(500);
                }

                updatedGame.GameName = updatedGameDto.GameName;
                updatedGame.GameTypeId = updatedGameDto.GameTypeId;
                updatedGame.Description = updatedGameDto.Description;
                updatedGame.Size = updatedGameDto.Size;

                Log(0, $"Game {id} updated.");

                _context.SaveChanges();

                return Ok(updatedGame);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while updating game {id}: {ex.Message}");
                return StatusCode(500);
            }
        }

        [HttpDelete("DeleteGame")]
        public ActionResult<GameDto> DeleteGame(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var gameToDelete = _context.Games.Include(x => x.GameGenres).FirstOrDefault(x => x.Idgame == id);
                if (gameToDelete == null)
                {
                    Log(0, $"Game {gameToDelete.Idgame} not found.");
                    return NotFound($"Could not find game with id {id}");
                }

                var reviewForGame = _context.Reviews.Where(x => x.GameId == id).ToList();

                foreach (var review in reviewForGame)
                {
                    _context.Reviews.Remove(review);
                }

                _context.GameGenres.RemoveRange(gameToDelete.GameGenres);

                _context.Games.Remove(gameToDelete);

                Log(0, $"Game {id} removed.");

                _context.SaveChanges();

                return Ok(gameToDelete);
            }
            catch (Exception ex)
            {
                Log(5, $"Error while removing game: {ex.Message}");
                return StatusCode(500);
            }
    }
}
}
