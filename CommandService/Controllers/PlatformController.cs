using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[ApiController]
[Route("api/commands/platforms")]
public class PlatformController : ControllerBase
{
    [HttpPost]
    public ActionResult AddPlatform()
    {
        return Ok("This is okay ");
    }
}
