using NovelTextProcessor.Dtos;

namespace WebApi.ObjectsMapper;

public static class EntityNameMapper
{
    public static IEnumerable<EntityName> ConvertFromModelEntityNameArrayToNovelTextProcessorEntityNameArray(
        Models.EntityName[] entityNames)
    {
        for (var i = 0; i < entityNames.Length; i++)
            yield return _ConvertFromModelEntityNameToNovelTextProcessorEntityName(entityNames[i]);
    }

    private static EntityName _ConvertFromModelEntityNameToNovelTextProcessorEntityName(Models.EntityName entityName)
    {
        return new EntityName
        {
            EnglishName = entityName.EnglishName,
            ArabicName = entityName.ArabicName!,
            Gender = entityName.Gender
        };
    }
}