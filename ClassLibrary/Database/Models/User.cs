namespace ClassLibrary.Database.Models;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string HashPassword { get; set; }
    public string? Email { get; set; }
    public bool IsVerifiedEmail { get; set; } = false;
    public string? EmailConfirmationToken { get; set; }
}
