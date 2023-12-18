using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Novel
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR"),MaxLength(300)]
        public required string NovelName { get; set; }
    }
}
