using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Database;

namespace EmailConfirmationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailConfirmationController : ControllerBase
{
    private readonly ApplicationContext _db;

    public EmailConfirmationController(ApplicationContext db)
    {
        _db = db;
    }

    [HttpGet("confirmEmail")]
    public IActionResult ConfirmEmail(string username, string token)
    {
        var user = _db.Users.FirstOrDefault(u => u.Name == username);
        if (user != null && user.EmailConfirmationToken == token)
        {
            user.IsVerifiedEmail = true;
            _db.SaveChanges();
            return Ok("Email подтвержден успешно.");
        }
        return BadRequest("Неверный токен подтверждения.");
    }
}
