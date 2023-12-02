using Catalyst;
using NovelTextProcessor.Dtos;
using NovelTextProcessor.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelTextProcessor
{
    public class Processor
    {
        string _novelText;
        EntityName[] _entityNames = Array.Empty<EntityName>();
        Document _document = null!;
        List<SpanWithEntityNames> _arrayOfspanWithEntityNames = null!;
        List<SpanWithEntityNames> _arrayOfTranslatedspanWithEntityNames = null!;


        public Processor(string text, EntityName[] entityNames)
        {
            _novelText = text;
            _entityNames = entityNames;

            DocumentProcessing();
            GenerateArrayOfSpanWithEntityNamesFromString();
            ReplaceAllEntityNamesToStaticNames("Oliver", "Maria");
            TranslateAllSpans(); //TDO translate and return Entity names to original one after translate using same method : ReplaceAllEntityNamesToStaticNames
            ConvertSpansToNormalText();
        }

        private void DocumentProcessing()
        {
            DocumentProcessor documentProcessor = new DocumentProcessor(_novelText.Trim());
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
            var indexOfText = _document.TokenizedValue().IndexOf(text);
            var spanWithEntityNames = new SpanWithEntityNames()
            {
                Span = text,
                IndexOfOriginalText = new() 
                {
                    From = indexOfText,
                    To = indexOfText + text.Length
                }
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

        private void ReplaceAllEntityNamesToStaticNames(string maleName,string femaleName)
        {
            for (int i = 0; i < _arrayOfspanWithEntityNames.Count; i++)
            {
                for (int t = 0; t < _arrayOfspanWithEntityNames[i].EntityNames.Count; t++)
                {
                    if (_arrayOfspanWithEntityNames[i].EntityNames[t].Gender == 'M')
                    {
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].Name, maleName);
                    }
                    else
                    {
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].Name, femaleName);
                    }
                }
            }
        }

        private void TranslateAllSpans()
        {
            var spans = _arrayOfspanWithEntityNames.Select(x => x.Span).ToList();
            var translatedSpans = TextTranslator.Instance.SendRequests(spans).GetAwaiter().GetResult().ToArray();
            
            for (int i = 0;i < translatedSpans.Length; i++)
            {
                _arrayOfTranslatedspanWithEntityNames.Add(new SpanWithEntityNames()
                {
                    Span = spans[i],
                    EntityNames = _arrayOfspanWithEntityNames[i].EntityNames,
                    IndexOfOriginalText = _arrayOfspanWithEntityNames[i].IndexOfOriginalText
                });
            }
        }

        private string ConvertSpansToNormalText() //TDO Edit all method to fix format to bycome same format of original document
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in _arrayOfspanWithEntityNames)
            {
                sb.AppendLine(item.Span);
            }
            return sb.ToString();
        }


        ~Processor()
        {
            ThreadSafeHttpClientSingleton.Instance.Dispose();
        }
    }
}
