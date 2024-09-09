using GameCriticBL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameCritic.MVCControllers
{
    public class LogItemController : Controller
    {
        private readonly RwaprojectDbContext _context;
        public LogItemController(RwaprojectDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllLogs")]
        public ActionResult<IEnumerable<LogItem>> GetAllLogs()
        {
            try
            {
                return Ok(_context.LogItems);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetLogById")]
        public ActionResult<LogItem> GetLogById(int id)
        {
            try
            {
                var log =
                    _context.LogItems
                    .FirstOrDefault(x => x.Idlog == id);
                if (log == null)
                {
                    return NotFound($"Could not find log with id {id}");
                }

                return Ok(log);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public IActionResult Index(int pageSize = 25)
        {
            var logs = _context.LogItems.Take(pageSize).ToList();
            ViewBag.PageSize = pageSize;
            return View(logs);
        }

        [HttpPost]
        public IActionResult Index(int pageSize, string action)
        {
            var logs = _context.LogItems.Take(pageSize).ToList();
            ViewBag.PageSize = pageSize;
            return View(logs);
        }
    }
}
