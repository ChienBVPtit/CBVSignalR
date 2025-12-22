using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CBVSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InboxEventController : ControllerBase
    {
        private readonly InboxEventService _inboxEventService;

        public InboxEventController(InboxEventService inboxEventService)
        {
            _inboxEventService = inboxEventService;
        }

        [HttpPost]
        //[AuthorizePermission("CREATE-GROUP")]
        public async Task<IActionResult> CreateInboxEvent([FromBody] InboxEvent request)
        {
            var p = await _inboxEventService.CreateAsync(request);
            return CreatedAtAction(null, new { id = p.Id }, p);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission("GET-GROUP-BY-ID")]
        public async Task<IActionResult> GetInboxEventById(Guid id)
        {
            var p = await _inboxEventService.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // GET: api/inboxEvent/
        [HttpGet]
        //[AuthorizePermission("GET-ALL-PERMISSION")]
        public async Task<IActionResult> GetAllInboxEvent()
        {
            var p = await _inboxEventService.GetAllAsync();
            if (p == null) return NotFound();
            return Ok(p);
        }

        // PUT: api/inboxEvent/{id}
        [HttpPut("{id}")]
        //[AuthorizePermission("UPDATE-PERMISSION")]
        public async Task<IActionResult> UpdateInboxEvent(Guid id, [FromBody] InboxEvent request)
        {
            var updated = await _inboxEventService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/inboxEvent/{id}
        [HttpDelete("{id}")]
        //[AuthorizePermission("DELETE-PERMISSION")]
        public async Task<IActionResult> DeleteInboxEvent(Guid id)
        {
            var deleted = await _inboxEventService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
