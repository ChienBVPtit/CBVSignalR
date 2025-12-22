using CBVSignalR.Application.Entities;
using CBVSignalR.Application.Interfaces;
using CBVSignalR.Application.Models;
using CBVSignalR.Application.Models.App;
using CBVSignalR.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CBVSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        //[AuthorizePermission("CREATE-GROUP")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification request)
        {
            var p = await _notificationService.CreateAsync(request);
            return CreatedAtAction(null, new { id = p.Id }, p);
        }

        [HttpGet("{id}")]
        //[AuthorizePermission("GET-GROUP-BY-ID")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            var p = await _notificationService.GetByIdAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        // GET: api/groupSubscription/
        [HttpGet("all")]
        //[AuthorizePermission("GET-ALL-PERMISSION")]
        public async Task<IActionResult> GetAllNotification()
        {
            var p = await _notificationService.GetAllAsync();
            if (p == null) return NotFound();
            return Ok(p);
        }

        // PUT: api/groupSubscription/{id}
        [HttpPut("{id}")]
        //[AuthorizePermission("UPDATE-PERMISSION")]
        public async Task<IActionResult> UpdateNotification(Guid id, [FromBody] Notification request)
        {
            var updated = await _notificationService.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/groupSubscription/{id}
        [HttpDelete("{id}")]
        //[AuthorizePermission("DELETE-PERMISSION")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var deleted = await _notificationService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] NotificationFilterRequest request)
        {
            return Ok(await _notificationService.GetAsync(request));
        }
    }
}
