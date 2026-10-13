using EF_DatabaseInheritance.Data;
using EF_DatabaseInheritance.DTOs;
using EF_DatabaseInheritance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EF_DatabaseInheritance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly EF_DatabaseInheritanceDbContext _context;

        public NotificationsController(EF_DatabaseInheritanceDbContext context)
        {
            _context = context;
        }

        // POST api/<NotificationsController>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Notification notification = dto.NotificationType.ToLower() switch
            {
                "email" => new EmailNotification
                {
                    Message = dto.Message,
                    UserId = dto.UserId,
                    Priority = dto.Priority!,
                    RecipientEmail = dto.RecipientEmail!,
                    Subject = dto.Subject!,
                    Cc = dto.Cc,
                    Bcc = dto.Bcc,
                    IsHtml = dto.IsHtml ?? true,
                    SentAt = DateTime.UtcNow
                },
                "sms" => new SmsNotification
                {
                    Message = dto.Message,
                    UserId = dto.UserId,
                    Priority = dto.Priority!,
                    RecipientNumber = dto.RecipientNumber!,
                    SenderId = dto.SenderId!,
                    Provider = dto.Provider ?? "Twilio",
                    SentAt = DateTime.UtcNow
                },
                "push" => new PushNotification
                {
                    Message = dto.Message,
                    UserId = dto.UserId,
                    Priority = dto.Priority!,
                    DeviceToken = dto.DeviceToken!,
                    AppPlatform = dto.AppPlatform!,
                    AppVersion = dto.AppVersion,
                    SentAt = DateTime.UtcNow
                },
                _ => throw new Exception("Invalid Notification Type")
            };  
            
            await _context.Notifications.AddAsync(notification);

            await _context.SaveChangesAsync();

            return Ok(new { Message = $"{dto.NotificationType} notification created.", Id = notification.Id });
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _context.Notifications.ToListAsync();

            var result = notifications.Select(n => new NotificationReadDTO
            {
                Id = n.Id,
                Message = n.Message,
                NotificationType = n.GetType().Name,
                SentAt = n.SentAt,
                IsDelivered = n.IsDelivered,
                Priority = n.Priority
            });
            return Ok(result);
        }

        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            IEnumerable<Notification> list = type.ToLower() switch
            {
                "email" => await _context.EmailNotifications.ToListAsync(),
                "sms" => await _context.SmsNotifications.ToListAsync(),
                "push" => await _context.PushNotifications.ToListAsync(),
                _ => new List<Notification>()
            };
            return Ok(list);
        }
    }
}
