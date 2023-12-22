using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class EntityName
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR"), MaxLength(300)]
        public required string Name { get; set; }

        [Column(TypeName = "VARCHAR"), AllowedValues('M','F')]
        public required char Gender { get; set; }

        //public int NovelId { get; set; }

        //public virtual required Novel Novel { get; set; }
    }
}
