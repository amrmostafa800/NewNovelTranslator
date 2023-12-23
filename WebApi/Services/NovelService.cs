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
				Id = n.Id,
				UserId = n.userId,
				Name = n.Novel!.novel_Name 
			}).ToList();
		}

		public async Task<Novel?> GetById(int id)
		{
			return await _context.Novels.FirstOrDefaultAsync(n => n.Id == id);
		}

		private async Task<int> _AddNovelIfNotExistElseReturnNovelId(string novelName)
		{
			var novel = await _context.Novels.FirstOrDefaultAsync(n => n.novel_Name == novelName);
			if (novel == null) 
			{
				novel = new Novel()
				{
					novel_Name = novelName
				};

				_context.Novels.Add(novel);
				_context.SaveChanges();
			}
			return novel.Id;
		}

		// i make add novel in 2 table to Allow Create Same Novel By Deffrant Users and Allow more than one User Acsses to Edit This Novel Or Add Entity Names To It without repeat novel name
		public async Task<int> AddNovel(string novelName,int UserIdWhoCreateNovel)
		{
			//Add Novel
			int novelId = await _AddNovelIfNotExistElseReturnNovelId(novelName);

			//Check If NovelUser Is Exist (this user already have novel with this name)
			if (_context.NovelUsers.Any(n => n.novelId == novelId && n.userId == UserIdWhoCreateNovel))
			{
				return 0;
			}
			//Add NovelUser
			var novelUser = new NovelUsers()
			{
				novelId = novelId,
				userId = UserIdWhoCreateNovel
			};

			await _context.NovelUsers.AddAsync(novelUser);

			await _context.SaveChangesAsync();
			return novelUser.Id; // here return NovelUser.id not novel.id
		}

		public async Task<bool> DeleteNovel(int id)
		{
			var novel = _context.NovelUsers.FirstOrDefault(n => n.Id == id);
			if (novel == null)
			{
				return false;
			}
			_context.NovelUsers.Remove(novel);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
