using System.Text;

namespace NovelTextProcessor.Dtos
{
    internal class SpanWithEntityNames
    {
        public string Span { get; set; } = null!;
        public List<EntityName> EntityNames { get; set; } = new List<EntityName>();
        public Index IndexOfOriginalText { get; set; } = null!;
    }
}
