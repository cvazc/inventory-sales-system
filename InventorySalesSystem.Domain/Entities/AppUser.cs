namespace InventorySalesSystem.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Clerk";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
