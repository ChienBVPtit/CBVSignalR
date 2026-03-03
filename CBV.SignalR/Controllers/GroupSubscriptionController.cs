using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models;
using CBVSignalR.Application.Models.App;
using CBVSignalR.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace CBVSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupSubscriptionController : ControllerBase
    {
        private readonly IGroupSubscriptionService _groupSubscriptionService;

        public GroupSubscriptionController(IGroupSubscriptionService groupSubscriptionService)
        {
            _groupSubscriptionService = groupSubscriptionService;
        }

        [HttpPost]
        //[AuthorizePermission("CREATE-GROUP")]
        public async Task<IActionResult> CreateGroupSubscription([FromBody] GroupSubscription request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code))
                return BadRequest("GroupName is required.");
            var exist = await _groupSubscriptionService.GetGroupSubscriptionByName(request.Name);
            if (exist != null) return Conflict();
            var p = await _groupSubscriptionService.CreateAsync(request);
            return CreatedAtAction(null, new { id = p.Id }, p);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission("GET-GROUP-BY-ID")]
        public async Task<IActionResult> GetGroupSubscriptionById(Guid id)
        {
            var p = await _groupSubscriptionService.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // GET: api/groupSubscription/
        [HttpGet("all")]
        //[AuthorizePermission("GET-ALL-PERMISSION")]
        public async Task<IActionResult> GetAllGroupSubscription()
        {
            var p = await _groupSubscriptionService.GetAllAsync();
            if (p == null) return NotFound();
            return Ok(p);
        }

        // PUT: api/groupSubscription/{id}
        [HttpPut("{id}")]
        //[AuthorizePermission("UPDATE-PERMISSION")]
        public async Task<IActionResult> UpdateGroupSubscription(Guid id, [FromBody] GroupSubscription request)
        {
            var updated = await _groupSubscriptionService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/groupSubscription/{id}
        [HttpDelete("{id}")]
        //[AuthorizePermission("DELETE-PERMISSION")]
        public async Task<IActionResult> DeleteGroupSubscription(Guid id)
        {
            var deleted = await _groupSubscriptionService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GroupSubscriptionFilterRequest request)
        {
            return Ok(await _groupSubscriptionService.GetAsync(request));
        }

    }
}
