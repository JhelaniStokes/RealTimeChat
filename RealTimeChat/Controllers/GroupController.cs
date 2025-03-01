using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealTimeChat.Hubs;
using RealTimeChat.Models;
using RealTimeChat.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace RealTimeChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public GroupController(AppDbContext dbContext, IHubContext<ChatHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(CreateGroupDto dto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            username = username.ToString();
            var group = new Models.Group()
            {
                GroupName = dto.GroupName,
                IsDm = dto.IsDm
            };
            _dbContext.Add(group);
            await _dbContext.SaveChangesAsync();
            int userId = int.Parse(User.FindFirst("uid")?.Value);
            // var useradded = AddUserToGroup(group.Id, userId);
            _dbContext.GroupMembers.Add(new GroupMember { GroupId = group.Id, UserId = userId });
            await _dbContext.SaveChangesAsync();
            await _hubContext.Clients.User(username).SendAsync("GroupAdded");//, group.Id, group.GroupName);
            return Ok(new { message = $"success groupId: {group.Id}" });
        }
        [HttpDelete("delete/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = await _dbContext.Groups.FindAsync(groupId);
            if (group == null)
            {
                return NotFound(new { message = "that group does not exist" });
            }
            _dbContext.Remove(group);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "success" });
        }
    
        [HttpGet("fetch")]
        public async Task<IActionResult> GetGroups() 
        {
            var userId = int.Parse(User.FindFirst("uid")?.Value);
            var groups = await _dbContext.GroupMembers
                .Where(gm => gm.UserId == userId)
                .Select(gm => gm.GroupChat)
                .ToListAsync();

            return Ok(groups);
        }
        [HttpPost("addUser/{groupId}/users/{username}")]
        public async Task<IActionResult> AddUserToGroup(int groupId, string username)
        {
            var group = await _dbContext.Groups.FindAsync(groupId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (group == null || user == null)
                return NotFound("Group or User not found");

            // Ensure user isn't already in the group
            var isMember = await _dbContext.GroupMembers
                .AnyAsync(gm => gm.GroupId == groupId && gm.UserId == user.Id);

            if (isMember)
                return BadRequest("User already in the group");

            // Add user to group
            _dbContext.GroupMembers.Add(new GroupMember { GroupId = groupId, UserId = user.Id });
            await _dbContext.SaveChangesAsync();

            // Notify the added user in real-time via SignalR
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("GroupAdded");//, groupId, group.GroupName);

            return Ok("User added to group successfully");
        }

        [HttpDelete("")]
        public async Task<IActionResult> RemoveFromGroup(int groupId, int userId)
        {
            var group = await _dbContext.Groups.FindAsync(groupId);
            var user = await _dbContext.Users.FindAsync(userId);

            if (group == null || user == null)
                return NotFound("Group or User not found");


            GroupMember groupMember = await _dbContext.GroupMembers.FirstOrDefaultAsync(g => g.GroupId == groupId && g.UserId == userId);

            if (groupMember == null)
                return BadRequest("User not in the group");

            // remove user to group
            
            _dbContext.GroupMembers.Remove(groupMember);
            await _dbContext.SaveChangesAsync();

            // Notify the added user in real-time via SignalR
            await _hubContext.Clients.User(userId.ToString()).SendAsync("GroupRemoved", groupId, group.GroupName);

            return Ok("User added to group successfully");
        }
    } 
} 
