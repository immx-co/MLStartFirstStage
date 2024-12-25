namespace ThirdStage.Database;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string HashPassword { get; set; }
    public string? Email { get; set; }
}
