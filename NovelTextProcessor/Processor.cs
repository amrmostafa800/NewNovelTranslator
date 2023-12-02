﻿using Catalyst;
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
        List<SpanWithEntityNames> _arrayOfspanWithEntityNames;
        List<SpanWithEntityNames> _arrayOfTranslatedspanWithEntityNames;


        public Processor(string text, EntityName[] entityNames)
        {
            _novelText = text;
            _entityNames = entityNames;
            _arrayOfspanWithEntityNames = new List<SpanWithEntityNames>();
            _arrayOfTranslatedspanWithEntityNames = new List<SpanWithEntityNames>();

            DocumentProcessing();
            GenerateArrayOfSpanWithEntityNamesFromString();
            ReplaceAllEntityNamesToStaticNames("Oliver", "Maria"); //TDO maybe add names to config file
            TranslateAllSpans(); //TDO translate and return Entity names to original one after translate
            ConvertSpansToNormalText();
        }

        private void DocumentProcessing()
        {
            DocumentProcessor documentProcessor = new DocumentProcessor(_novelText.Trim());
            _document = documentProcessor.document;
        }

        private void GenerateArrayOfSpanWithEntityNamesFromString()
        {
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
                if (text.Contains(_entityNames[i].EnglishName))
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
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].EnglishName, maleName);
                    }
                    else
                    {
                        _arrayOfspanWithEntityNames[i].Span = _arrayOfspanWithEntityNames[i].Span.ReplaceFirst(_arrayOfspanWithEntityNames[i].EntityNames[t].EnglishName, femaleName);
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
                    Span = translatedSpans[i],
                    EntityNames = _arrayOfspanWithEntityNames[i].EntityNames,
                    IndexOfOriginalText = _arrayOfspanWithEntityNames[i].IndexOfOriginalText
                });
            }
        }

        private string ConvertSpansToNormalText() //TDO Edit all method to fix format to bycome same format of original document + Find Better Name For Method
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
