namespace NovelTextProcessor.Dtos;

internal class SpanAndEntityNames
{
    public SpanAndEntityNames()
    {
    }

    public SpanAndEntityNames(SpanAndEntityNames spanAndEntityNames)
    {
        Span = spanAndEntityNames.Span;
        EntityNames = spanAndEntityNames.EntityNames;
    }

    public string Span { get; set; } = null!;
    public List<EntityName> EntityNames { get; set; } = new();
}