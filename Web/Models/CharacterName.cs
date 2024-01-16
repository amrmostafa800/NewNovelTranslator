namespace Web.Models;

public record CharacterName
{
    public int Id { get; set; }
    public required string englishName { get; set; }
    public required string arabicName { get; set; }
    public required string gender { get; set; }
}