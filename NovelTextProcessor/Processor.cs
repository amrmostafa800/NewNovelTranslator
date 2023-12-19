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

			_Worker();
		}

		private void _Worker()
		{
			var englishSpans = _SplitTextToSpans(5000);

			_MapEnglishSpansToListOfSpanAndEntityNames(ref englishSpans, ref listOfSpanAndEntityNames);
			_SearchForEntityNamesInSpans(ref listOfSpanAndEntityNames);
			_ReplaceAllEntityNamesInSpanToFixedName(ref listOfSpanAndEntityNames);
			_TranslateAllSpans(ref listOfSpanAndEntityNames); // now spans in listOfSpanAndEntityNames is translated but names is FixedNames and listOfSpanAndEntityNames.EntityName have original names
			_RestoreOriginalNames(ref listOfSpanAndEntityNames); // now spans ready
		}

		private void _RestoreOriginalNames(ref List<SpanAndEntityNames> listOfSpanAndEntityNames)
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

		private void _TranslateAllSpans(ref List<SpanAndEntityNames> listOfSpanAndEntityNames)
		{
			var spans = listOfSpanAndEntityNames.Select(x => x.Span).ToList();
			var translatedSpans = Task.Run(() => TextTranslator.Instance.SendRequests(spans).GetAwaiter().GetResult().ToArray()).Result; //TDO Find Better Why Other Than Create New Thered

			for (int i = 0; i < translatedSpans.Length; i++)
			{
				listOfSpanAndEntityNames[i].Span = translatedSpans[i];
			}
		}

		private void _ReplaceAllEntityNamesInSpanToFixedName(ref List<SpanAndEntityNames> listOfSpanAndEntityNames)
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

		private void _MapEnglishSpansToListOfSpanAndEntityNames(ref string[] englishSpans, ref List<SpanAndEntityNames> listOfSpanAndEntityNames)
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
		private void _SearchForEntityNamesInSpans(ref List<SpanAndEntityNames> listOfSpanAndEntityNames)
		{
			for (int i = 0; i < listOfSpanAndEntityNames.Count; i++)
			{
				listOfSpanAndEntityNames[i].EntityNames = _SearchForEntityNamesInSpan(new(listOfSpanAndEntityNames[i]));
			}
		}

		private List<EntityName> _SearchForEntityNamesInSpan(SpanAndEntityNames spanAndEntityNames)
		{
			while (true)
			{
				var currentEntityName = _GetFirstEntityNameInSpan(spanAndEntityNames.Span);

				if (currentEntityName == null)
					break;

				spanAndEntityNames.EntityNames.Add(currentEntityName);
				spanAndEntityNames.Span = spanAndEntityNames.Span.ReplaceFirst(currentEntityName.EnglishName, ""); // remove from span
				currentEntityName = null;
			}


			return spanAndEntityNames.EntityNames;
		}

		private EntityName? _GetFirstEntityNameInSpan(string span)
		{
			foreach (var entityName in _entityNames)
			{
				if (span.Contains(entityName.EnglishName))
				{
					return entityName;
				}
			}
			return null;
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

		//private int _CountHowMuchEntityNamesInSpan(string englishSpan)
		//{
		//    int count = 0;
		//    for (int i = 0; i < _entityNames.Length; i++)
		//    {
		//        while (englishSpan.Contains(_entityNames[i].EnglishName))
		//        {
		//            count++;
		//            englishSpan = englishSpan.ReplaceFirst(_entityNames[i].EnglishName, ""); // remove after count to dont count again
		//        }
		//    }
		//    return count;
		//}

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
