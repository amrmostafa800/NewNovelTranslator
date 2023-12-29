using Catalyst;
using Catalyst.Models;
using Mosaik.Core;

namespace NovelTextProcessor
{
	public class Document_NLP
	{
		Pipeline _nlp;
		public Document document = null!;
		public Document_NLP()
		{
			English.Register();
			_nlp = Pipeline.For(Language.English);
		}

		public async Task RunAsync(string text)
		{
			_nlp.Add(await AveragePerceptronEntityRecognizer.FromStoreAsync(language: Language.English, version: Mosaik.Core.Version.Latest, tag: "WikiNER"));
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
