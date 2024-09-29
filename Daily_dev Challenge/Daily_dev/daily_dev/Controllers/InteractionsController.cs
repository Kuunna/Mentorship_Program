using daily_dev.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace daily_dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractionsController : ControllerBase
    {
        private readonly NewsDbContext _context;

        public InteractionsController(NewsDbContext context)
        {
            _context = context;
        }

        [HttpPost("bookmark")]
        public async Task<IActionResult> Bookmark(Fact_Bookmark bookmark)
        {
            _context.Fact_Bookmarks.Add(bookmark);
            await _context.SaveChangesAsync();
            return Ok("News bookmarked");
        }

        [HttpPost("like")]
        public async Task<IActionResult> Like(Fact_Article_Interaction interaction)
        {
            interaction.InteractionType = "Like";
            _context.Fact_Article_Interactions.Add(interaction);
            await _context.SaveChangesAsync();
            return Ok("News liked");
        }

        [HttpPost("comment")]
        public async Task<IActionResult> Comment(Fact_Comments comment)
        {
            _context.Fact_Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok("Comment added");
        }
    }

}
