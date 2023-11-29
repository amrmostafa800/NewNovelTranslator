using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelTextProcessor.Dtos
{
    public class EntityName
    {
        public string Name { get; set; } = null!;

        [AllowedValues('M','F')]
        public Char Gender { get; set; }
    }
}
