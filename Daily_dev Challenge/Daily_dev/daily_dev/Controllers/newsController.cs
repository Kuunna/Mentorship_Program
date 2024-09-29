using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public NewsController(NewsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var newsList = await _context.Fact_News.ToListAsync();
            return Ok(newsList);
        }

        [HttpGet("{newsId}")]
        public async Task<IActionResult> GetNewsById(int newsId)
        {
            var news = await _context.Fact_News.FindAsync(newsId);
            if (news == null)
                return NotFound();
            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(Fact_News news)
        {
            _context.Fact_News.Add(news);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNewsById), new { newsId = news.NewID }, news);
        }
    }

}
