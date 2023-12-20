using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class Novel
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR"),MaxLength(300)]
        public required string NovelName { get; set; }

        public int UserId { get; set; }

        public CustomIdentityUser? User { get; set; }
    }
}
