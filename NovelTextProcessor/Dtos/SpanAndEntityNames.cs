using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelTextProcessor.Dtos
{
    internal class SpanAndEntityNames
    {
        public string Span { get; set; } = null!;
        public List<EntityName> EntityNames { get; set; } = new List<EntityName>();


        public SpanAndEntityNames() 
        {
        }
        public SpanAndEntityNames(SpanAndEntityNames spanAndEntityNames)
        {
            Span = spanAndEntityNames.Span;
            EntityNames = spanAndEntityNames.EntityNames;
        }
    }
}
