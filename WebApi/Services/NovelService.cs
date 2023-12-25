using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services
{
	public class NovelService
	{
		private readonly ApplicationDbContext _context;

		public NovelService(ApplicationDbContext context)
		{
			_context = context;
		}

		public List<NovelDto> GetAllNovels() 
		{
			return _context.NovelUsers.Select(n => new NovelDto
			{
				Id = n.NovelId,
				UserId = n.UserId,
				Name = n.Novel!.NovelName!.novelName 
			}).ToList();
		}

		public NovelDto? GetById(int id)
		{
			return _context.NovelUsers.Where(n => n.NovelId == id).Select(n => new NovelDto
			{
				Id = n.NovelId,
				UserId = n.UserId,
				Name = n.Novel!.NovelName!.novelName
			}).FirstOrDefault();
		}

		private async Task<int> _AddNovelNameIfNotExistElseReturnNovelNameId(string novelName)
		{
			var novelNameInfo = await _context.NovelNames.FirstOrDefaultAsync(n => n.novelName == novelName);
			if (novelNameInfo == null) 
			{
				novelNameInfo = new NovelName()
				{
					novelName = novelName
				};

				_context.NovelNames.Add(novelNameInfo);
				_context.SaveChanges();
			}
			return novelNameInfo.Id;
		}

		// i make add novel in 2 table to Allow Create Same Novel By Deffrant Users and Allow more than one User Acsses to Edit This Novel Or Add Entity Names To It without repeat novel name
		public async Task<int> AddNovel(string novelName, int UserIdWhoCreateNovel)
		{
			//Add NovelName
			int novelNameId = await _AddNovelNameIfNotExistElseReturnNovelNameId(novelName);

			//Check If Novel Is Exist (this user already have novel with this name)
			if (_context.NovelUsers.Any(n => n.Novel!.NovelNameId == novelNameId && n.UserId == UserIdWhoCreateNovel))
			{
				return 0;
			}
			//Add Novel
			var novelClone = new Novel()
			{
				NovelNameId = novelNameId,
			};

			await _context.Novels.AddAsync(novelClone);

			await _context.SaveChangesAsync();

			//Add NovelUser
			var novel = new NovelUser()
			{
				NovelId = novelClone.Id,
				UserId = UserIdWhoCreateNovel,
			};

			await _context.NovelUsers.AddAsync(novel);

			await _context.SaveChangesAsync();
			return novelClone.Id;
		}

		public async Task<bool> DeleteNovel(int id)
		{
			//Try Get Novel
			var novelClone = _context.Novels.FirstOrDefault(n => n.Id == id);
			if (novelClone == null)
			{
				return false;
			}

			_context.Novels.Remove(novelClone); // Bycouse Cascade Delete Enabled Remove Novel Will Remove Novel Too
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> IsUserOwnThisNovel(int novelId,int UserId)
		{
			var novel = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == UserId);
			return novel != null;
		} 
	}
}
