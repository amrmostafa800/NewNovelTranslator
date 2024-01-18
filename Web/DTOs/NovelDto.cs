namespace Web.Models;

public class NovelDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
}