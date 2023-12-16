using Catalyst;
using Catalyst.Models;
using Mosaik.Core;

namespace NovelTextProcessor
{
    public class Document_NLP
    {
        Pipeline _nlp;
        public Document document;
        public Document_NLP(string text)
        {
            English.Register();
            _nlp = Pipeline.For(Language.English);
            _nlp.Add(AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.English, version: Mosaik.Core.Version.Latest, tag: "WikiNER").Result);
            document = new Document(text.Trim(), Language.English);
            _nlp.ProcessSingle(document);
        }

        public string[] ExtractEntityNames()
        {
            return document.SelectMany(span => span.GetEntities())
            .Where(e => e.EntityType.Type == "Person" || e.EntityType.Type == "Organization")
            .Select(e => e.Value).Distinct().ToArray();
        }
    }
}
