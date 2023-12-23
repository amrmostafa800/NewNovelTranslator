using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class NovelName
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR"), MaxLength(256)]
        public required string novelName { get; set; }
    }
}
