namespace NovelTextProcessor.Dtos
{
    internal class TranslationRequestDto
    {
        public bool multiline { get; set; }
        public string source { get; set; } = null!;
        public string target { get; set; } = null!;
        public string q { get; set; } = null!;
        public string hints { get; set; } = null!;
        public long ts { get; set; }
        public string verify { get; set; } = null!;
    }
}
