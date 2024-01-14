using Microsoft.EntityFrameworkCore;
using NovelTextProcessor;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services;

public class EntityNameService
{
    private readonly ApplicationDbContext _context;

    public EntityNameService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<EntityName> GetEntityNamesByNovelId(int novelId)
    {
        return _context.EntityNames.Where(n => n.NovelId == novelId).OrderBy(n => n.EnglishName.Length).ToList();
    }

    public async Task<bool> AddManyEntityNames(EntityNameDto entityNameDto)
    {
        //Check If any EntityName Already Exist
        var englishNamesToCheck = entityNameDto.EntityNames.Select(en => en.EnglishName).ToList();

        var isAnyEntityNameExistBefore = await _context.EntityNames
            .AnyAsync(e => englishNamesToCheck.Contains(e.EnglishName) && e.NovelId == entityNameDto.NovelId);

        if (isAnyEntityNameExistBefore) // if found EntityName Exist return false
            return false;

        //Add EntityNames
        var entityNamesTask = entityNameDto.EntityNames.Select(async e => new EntityName
        {
            EnglishName = e.EnglishName,
            ArabicName = await TextTranslator.Instance.SendRequestAsync(e.EnglishName), // Translate English Name To Arabic
            Gender = e.Gender,
            NovelId = entityNameDto.NovelId
        }).ToList();
        var entityNames = await Task.WhenAll(entityNamesTask);

        _context.EntityNames.AddRange(entityNames);
        if (await _context.SaveChangesAsync() != 0) return true;
        return false; // failed
    }

    //public async Task<int> AddEntityName(string enName, char gender, int novelId)
    //{
    //	var entityName = new EntityName()
    //	{
    //		EnglishName = enName,
    //		ArabicName = await TextTranslator.Instance.SendRequestAsync(enName),
    //		Gender = gender,
    //		NovelId = novelId,
    //	};

    //	_context.EntityNames.Add(entityName);
    //	if (await _context.SaveChangesAsync() != 0)
    //	{
    //		return entityName.Id;
    //	}
    //	return 0; // failed
    //}

    public async Task<bool> UpdateEntityName(int id, string newEnglishName, char gender)
    {
        var entityName = _context.EntityNames.FirstOrDefault(n => n.Id == id);
        if (entityName == null) return false;
        entityName.EnglishName = newEnglishName;
        entityName.Gender = gender;
        entityName.ArabicName = await TextTranslator.Instance.SendRequestAsync(newEnglishName);

        _context.EntityNames.Update(entityName);
        await _context.SaveChangesAsync();
        return true;
    }

    public bool DeleteEntityName(int entityNameId)
    {
        return _context.EntityNames.Where(e => e.Id == entityNameId).ExecuteDelete() != 0;
    }

    public bool
        CheckIfNovelUserIdsOfThisEntityNameEqualThisNovelUserId(int entityNameId,
            int currentAuthUserId) // i do it with this why bycouse novel can have more than 1 user
    {
        var query = from entityName in _context.EntityNames
            join novelUser in _context.NovelUsers on entityName.NovelId equals novelUser.NovelId
            where entityName.Id == entityNameId && novelUser.UserId == currentAuthUserId
            select entityName.Id;

        return query.Any();
    }
}