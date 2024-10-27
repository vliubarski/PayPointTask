using Microsoft.AspNetCore.Mvc;

namespace PayPoint.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargeNotificationController : ControllerBase
{
    private readonly ILogger<ChargeNotificationController> _logger;

    public ChargeNotificationController(ILogger<ChargeNotificationController> logger)
    {
        _logger = logger;
    }

    [HttpGet("test")]
    public IEnumerable<int> Get()
    {
        return Enumerable.Range(1, 4);
    }
}
