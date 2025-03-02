using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Models;
using System.Threading.Tasks;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        public ChatController(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet("fetchmessages/{groupId}")]
        public async Task<IActionResult> GetMessages(int groupId)
        {
            var userId = int.Parse(User.FindFirst("uid")?.Value);
            var messages = await _dbcontext.Messages.Where(m => m.GroupId == groupId).ToListAsync();
            messages = messages.OrderBy(m => m.SentAt).ToList();
            return Ok(messages);
        }
    }
}
