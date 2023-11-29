using Catalyst;
using NovelTextProcessor.Dtos;
using NovelTextProcessor.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelTextProcessor
{
    public class Processor
    {
        StringBuilder _novelText = new StringBuilder();
        EntityName[] _entityNames = Array.Empty<EntityName>();
        Document _document = null!;
        List<SpanWithEntityNames> _arrayOfspanWithEntityNames = null!;

        public void Process(string text, EntityName[] entityNames) 
        {
            _novelText.Append(text);

            ExtractEntityNamesAndSpans(entityNames);
            GenerateArrayOfSpanWithEntityNamesFromString();
            ReplaceAllEntityNamesToStaticNames();
        }

        private void ExtractEntityNamesAndSpans(EntityName[] entityNames)
        {
            DocumentProcessor documentProcessor = new DocumentProcessor(_novelText.ToString().Trim());
            _entityNames = entityNames;
            _document = documentProcessor.document;
        }

        private void GenerateArrayOfSpanWithEntityNamesFromString()
        {
            _arrayOfspanWithEntityNames = new List<SpanWithEntityNames>();

            foreach (var span in _document)
            {
                _arrayOfspanWithEntityNames.Add(GenerateSpanWithEntityNamesFromString(span.TokenizedValue));
            }
        }

        private SpanWithEntityNames GenerateSpanWithEntityNamesFromString(string text)
        {
            var spanWithEntityNames = new SpanWithEntityNames()
            {
                Span = text,
            };

            for (int i = 0; i < _entityNames.Length; i++)
            {
                if (text.Contains(_entityNames[i].Name))
                {
                    spanWithEntityNames.EntityNames.Add(_entityNames[i]);
                }
            }
            return spanWithEntityNames;
        }

        private void ReplaceAllEntityNamesToStaticNames()
        {
            for (int i = 0; i < _arrayOfspanWithEntityNames.Count; i++)
            {
                for (int t = 0; t < _arrayOfspanWithEntityNames[i].EntityNames.Count; t++)
                {
                    if (_arrayOfspanWithEntityNames[i].EntityNames[t].Gender == 'M')
                    {
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].Name, "Oliver");
                    }
                    else
                    {
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].Name, "Maria");
                    }
                }
            }
        }

        //private int CheckHowMuchEntityNamesInString(string text)
        //{
        //    if (string.IsNullOrEmpty(text)) return 0;

        //    int counter = 0;

        //    for (int i = 0; i < entityNames.Length; i++) 
        //    {
        //        if (text.Contains(entityNames[i]))
        //        {
        //            counter++;
        //        }
        //    }
        //    return counter;
        //}
    }
}
