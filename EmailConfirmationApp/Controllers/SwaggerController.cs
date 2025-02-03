using Microsoft.AspNetCore.Mvc;

namespace EmailConfirmationApp.Controllers
{
    public class SwaggerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Message = "Hello from Swagger!" });
        }
    }
}
