using Microsoft.EntityFrameworkCore;
using NovelTextProcessor;
using System.Xml.Linq;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services
{
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

		public async Task<int> AddEntityName(string enName,char gender,int novelId)
		{
			var entityName = new EntityName()
			{
				EnglishName = enName,
				ArabicName = await TextTranslator.Instance.SendRequestAsync(enName),
				Gender = gender,
				NovelId = novelId,
			};

			_context.EntityNames.Add(entityName);
			if (await _context.SaveChangesAsync() != 0)
			{
				return entityName.Id;
			}
			return 0; // failed
		}

		public async Task<bool> UpdateEntityName(int id, string newEnglishName, char gender)
		{
			var entityName = _context.EntityNames.FirstOrDefault(n => n.Id == id);
			if (entityName == null) 
			{
				return false;
			}
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
	}
}
