using System.ComponentModel.DataAnnotations;

namespace NovelTextProcessor.Dtos;

public class EntityName
{
    public string EnglishName { get; set; } = null!;
    public string ArabicName { get; set; } = null!;

    [AllowedValues('M', 'F')] public char Gender { get; set; }
}