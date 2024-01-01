using NovelTextProcessor.Dtos;
using NovelTextProcessor.Extensions;
using System.Text;

namespace NovelTextProcessor
{
	public class Processor
	{
		string _novelText;
		EntityName[] _entityNames = null!;
		List<SpanAndEntityNames> listOfSpanAndEntityNames = new();

		string FixedEnMaleName = "Oliver";
		string FixedArMaleName = "أوليفر";
		string FixedEnFemaleName = "Maria";
		string FixedArFemaleName = "ماريا";

		public Processor(string novelText, EntityName[] entityNames)
		{
			_novelText = novelText;
			_entityNames = entityNames;
		}

		public async Task RunAsync()
		{
			var englishSpans = _SplitTextToSpans(5000);

			_MapEnglishSpansToListOfSpanAndEntityNames(ref englishSpans);
			_SearchForEntityNamesInSpans();
			_ReplaceAllEntityNamesInSpanToFixedName();
			await TranslateAllSpansAsync(); // now spans in listOfSpanAndEntityNames is translated but names is FixedNames and listOfSpanAndEntityNames.EntityName have original names
			_RestoreOriginalNames(); // now spans ready
		}

		private void _RestoreOriginalNames()
		{
			for (int i = 0; i < listOfSpanAndEntityNames.Count; i++)
			{
				for (int t = 0; t < listOfSpanAndEntityNames[i].EntityNames.Count; t++)
				{
					if (listOfSpanAndEntityNames[i].EntityNames[t].Gender == 'M')
					{
						listOfSpanAndEntityNames[i].Span = listOfSpanAndEntityNames[i].Span.ReplaceFirst(FixedArMaleName, listOfSpanAndEntityNames[i].EntityNames[t].ArabicName);
					}
					else
					{
						listOfSpanAndEntityNames[i].Span = listOfSpanAndEntityNames[i].Span.ReplaceFirst(FixedArFemaleName, listOfSpanAndEntityNames[i].EntityNames[t].ArabicName);
					}
				}
			}
		}

		private async Task TranslateAllSpansAsync()
		{
			var spans = listOfSpanAndEntityNames.Select(x => x.Span).ToList();
			var pretranslateSpans = await TextTranslator.Instance.SendRequestsAsync(spans);
			var translatedSpans = pretranslateSpans.ToArray();

			for (int i = 0; i < translatedSpans.Length; i++)
			{
				listOfSpanAndEntityNames[i].Span = translatedSpans[i];
			}
		}

		private void _ReplaceAllEntityNamesInSpanToFixedName()
		{
			for (int i = 0; i < listOfSpanAndEntityNames.Count; i++)
			{
				foreach (var entityName in listOfSpanAndEntityNames[i].EntityNames)
				{
					if (entityName.Gender == 'M')
					{
						listOfSpanAndEntityNames[i].Span = listOfSpanAndEntityNames[i].Span.Replace(entityName.EnglishName, FixedEnMaleName);
					}
					else
					{
						listOfSpanAndEntityNames[i].Span = listOfSpanAndEntityNames[i].Span.Replace(entityName.EnglishName, FixedEnFemaleName);
					}
				}
			}
		}

		private void _MapEnglishSpansToListOfSpanAndEntityNames(ref string[] englishSpans)
		{
			foreach (var englishSpan in englishSpans)
			{
				listOfSpanAndEntityNames.Add(new SpanAndEntityNames()
				{
					Span = englishSpan
				});
			}
		}

		/// <summary>
		/// Search In All List of SpanAndEntityNames If Found Span Contain EntityName Than Add it To SpanAndEntityNames.EntityNames
		/// </summary>
		/// <param name="listOfSpanAndEntityNames"></param>
		private void _SearchForEntityNamesInSpans()
		{
			for (int i = 0; i < listOfSpanAndEntityNames.Count; i++)
			{
				listOfSpanAndEntityNames[i].EntityNames = _SearchForEntityNamesInSpan(new(listOfSpanAndEntityNames[i]));
			}
		}

		private List<EntityName> _SearchForEntityNamesInSpan(SpanAndEntityNames spanAndEntityNames)
		{
			var entityNamesWithIndex = new Dictionary<int, EntityName>();
			StringBuilder SpanAsStringBuilder = new StringBuilder(spanAndEntityNames.Span);

			int currentIdex = 0;

			for (int i = 0; i < _entityNames.Length; i++)
			{
				while (true)
				{
					currentIdex = SpanAsStringBuilder.ToString().IndexOf(_entityNames[i].EnglishName, currentIdex + 1);// try find index of entity name if not found its return -1
																													   //SpanAsStringBuilder.ReplaceFromTwoIndexToNewText(0, currentIdex + _entityNames[i].EnglishName.Length,""); // remove text from begining to entity name from string builder
					if (currentIdex != -1) // check if found
					{
						entityNamesWithIndex.Add(currentIdex, _entityNames[i]);
					}
					else
					{
						break; // no more from this entity name in span
					}
				}
			}

			//create SpanAndEntityNames.entitynames again to sort correct (low index first)
			return spanAndEntityNames.EntityNames = entityNamesWithIndex.OrderBy(e => e.Key).Select(e => e.Value).ToList();
		}

		protected string[] _SplitTextToSpans()
		{
			// Split
			List<string> spans = _novelText.Split(Environment.NewLine).ToList();

			//Remove Empty Lines
			for (int i = 0; i < spans.Count; i++)
			{
				if (spans[i] == string.Empty)
				{
					spans.RemoveAt(i);
				}
			}

			return spans.ToArray();
		}

		//This Method By Ai (at its time i dont know about LastIndexOf) It was originaly in other project by me and i copy from it to use it here
		protected string[] _SplitTextToSpans(int maxLength)
		{
			List<string> stringList = new List<string>();

			// Split the input string into substrings of maximum length 2000
			while (_novelText.Length > maxLength)
			{
				// Find the nearest '.' character to the maximum length position
				int splitPos = _novelText.LastIndexOf('.', maxLength - 1);
				if (splitPos == -1)
				{
					// No '.' character found, split at the maximum length position
					splitPos = maxLength;
				}
				// Add the substring to the list
				stringList.Add(_novelText.Substring(0, splitPos + 1));
				// Remove the substring from the input string
				_novelText = _novelText.Substring(splitPos + 1);
			}
			// Add the remaining substring to the list
			stringList.Add(_novelText);

			// Convert the list to an array
			return stringList.ToArray();
		}

		private string _GenerateTextAfterAllEdits()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < listOfSpanAndEntityNames.Count; i++)
			{
				sb.AppendLine(listOfSpanAndEntityNames[i].Span);
			}
			return sb.ToString().Trim();
		}
		public string GetResult()
		{
			return _GenerateTextAfterAllEdits();
		}
	}
}
