using WebApi.Models;

namespace WebApi.ObjectsMapper
{
	public static class EntityNameMapper
	{
		public static IEnumerable<NovelTextProcessor.Dtos.EntityName> ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(EntityName[] entityNames)
		{
			for (int i = 0; i < entityNames.Length; i++)
			{
				EntityName? entityName = entityNames[i];
				yield return _ConvertFromModelEntityNameToNovelTextProcessorEntityName(entityName);
			}
		}

		private static NovelTextProcessor.Dtos.EntityName _ConvertFromModelEntityNameToNovelTextProcessorEntityName(EntityName entityName)
		{
			return new NovelTextProcessor.Dtos.EntityName()
			{
				EnglishName = entityName.EnglishName,
				ArabicName = entityName.ArabicName!,
				Gender = entityName.Gender,
			};
		}
	}
}
