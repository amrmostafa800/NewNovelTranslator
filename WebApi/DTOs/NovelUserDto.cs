namespace WebApi.DTOs;

public class NovelUserDto
{
    public int NovelId { get; set; }
    public int NovelUserId { get; set; }
    public required string UserName { get; set; }
    
}