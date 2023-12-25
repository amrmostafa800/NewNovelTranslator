using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
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

		public int AddEntityName(string enName,char gender,int novelId)
		{
			var entityName = new EntityName()
			{
				EnglishName = enName,
				ArabicName = "TDO", //TDO translate english Name here To Get Arabic Name
				Gender = gender,
				NovelId = novelId,
			};

			_context.EntityNames.Add(entityName);
			if (_context.SaveChanges() != 0)
			{
				return entityName.Id;
			}
			return 0; // failed
		}

		public bool UpdateEntityName(int id,string NewEnglishName)
		{
			var entityName = _context.EntityNames.FirstOrDefault(n => n.Id == id);
			if (entityName == null) 
			{
				return false;
			}
			entityName.EnglishName = NewEnglishName;
			entityName.ArabicName = "TDO"; //TDO translate english Name here To Get Arabic Name
			return true;
		}

		public bool DeleteEntityName(int entityNameId) 
		{
			return _context.EntityNames.Where(e => e.Id == entityNameId).ExecuteDelete() != 0;
		}
	}
}
