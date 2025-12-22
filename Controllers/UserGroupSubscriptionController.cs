using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models;
using CBVSignalR.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CBVSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupSubscriptionController : Controller
    {
        private readonly UserGroupSubscriptionService _userGroupSubscriptionService;

        public UserGroupSubscriptionController(UserGroupSubscriptionService userGroupSubscriptionService)
        {
            _userGroupSubscriptionService = userGroupSubscriptionService;
        }

        [HttpPost]
        //[AuthorizePermission("CREATE-GROUP")]
        public async Task<IActionResult> CreateUserGroupSubscription([FromBody] UserGroupSubscription request)
        {
            var p = await _userGroupSubscriptionService.CreateAsync(request);
            return CreatedAtAction(null, new { id = p.Id }, p);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission("GET-GROUP-BY-ID")]
        public async Task<IActionResult> GetUserGroupSubscriptionById(Guid id)
        {
            var p = await _userGroupSubscriptionService.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // GET: api/userGroupSubscription/
        [HttpGet("all")]
        //[AuthorizePermission("GET-ALL-PERMISSION")]
        public async Task<IActionResult> GetAllUserGroupSubscription()
        {
            var p = await _userGroupSubscriptionService.GetAllAsync();
            if (p == null) return NotFound();
            return Ok(p);
        }

        // PUT: api/userGroupSubscription/{id}
        [HttpPut("{id}")]
        //[AuthorizePermission("UPDATE-PERMISSION")]
        public async Task<IActionResult> UpdateUserGroupSubscription(Guid id, [FromBody] UserGroupSubscription request)
        {
            var updated = await _userGroupSubscriptionService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/userGroupSubscription/{id}
        [HttpDelete("{id}")]
        //[AuthorizePermission("DELETE-PERMISSION")]
        public async Task<IActionResult> DeleteUserGroupSubscription(Guid id)
        {
            var deleted = await _userGroupSubscriptionService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingFilterRequest request)
        {
            return Ok(await _userGroupSubscriptionService.GetAsync(request));
        }
    }
}
