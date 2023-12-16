namespace NovelTextProcessor.Dtos
{
    internal class SpanWithEntityNames
    {
        public string EnglishSpan { get; set; } = null!;
        public string ArabicSpan { get; set; } = null!;
        public List<EntityName> EntityNames { get; set; } = new List<EntityName>();
        public Index IndexOfOriginalText { get; set; } = null!;
    }
}
