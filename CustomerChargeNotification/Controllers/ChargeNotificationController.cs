using Microsoft.AspNetCore.Mvc;
using CustomerChargeNotification.Services;

namespace CustomerChargeNotification.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargeNotificationController : ControllerBase
{
    private readonly IChargeNotificationService _notificationService;
    private readonly ILogger<ChargeNotificationController> _logger;

    public ChargeNotificationController(IChargeNotificationService notificationService,
        ILogger<ChargeNotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateChargeNotifications()
    {
        await _notificationService.GenerateChargeNotifications(new DateTime(2021, 5, 21));
        return Ok("Charge notifications generated.");
    }
}
