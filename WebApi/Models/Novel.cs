using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

public class Novel
{
    public int Id { get; set; }

    [Column(TypeName = "VARCHAR")]
    [MaxLength(256)]
    public required string NovelName { get; set; }
}