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
				UserName = n.User!.UserName!,
				Name = n.Novel!.NovelName!.novelName
			}).ToList();
		}

		public NovelDto? GetById(int id)
		{
			return _context.NovelUsers.Where(n => n.NovelId == id).Select(n => new NovelDto
			{
				Id = n.NovelId,
				UserId = n.UserId,
				UserName = n.User!.UserName!,
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

		private async Task<bool> _AddNovelUser(int novelId,int userId)
		{
			var novelUser = new NovelUser()
			{
				NovelId = novelId,
				UserId = userId,
			};

			await _context.NovelUsers.AddAsync(novelUser);

			return await _context.SaveChangesAsync() != 0; // check if Added Successfully
		}

		private async Task<Novel> _AddNovel(int novelNameId)
		{
			var novel = new Novel()
			{
				NovelNameId = novelNameId,
			};

			await _context.Novels.AddAsync(novel);

			return novel; // check if Added Successfully
		}

		private bool _CheckIfNovelAlreadyExistWithThisUser(int novelNameId,int userId)
		{
			if (_context.NovelUsers.Any(n => n.Novel!.NovelNameId == novelNameId && n.UserId == userId))
			{
				return true;
			}
			return false;
		}

		// i make add novel in 2 table to Allow Create Same Novel By Deffrant Users and Allow more than one User Acsses to Edit This Novel Or Add Entity Names To It without repeat novel name
		public async Task<int> AddNovel(string novelName, int UserIdWhoCreateNovel)
		{
			//Add NovelName
			int novelNameId = await _AddNovelNameIfNotExistElseReturnNovelNameId(novelName);

			//Check If Novel Is Exist (this user already have novel with this name)
			if (_CheckIfNovelAlreadyExistWithThisUser(novelNameId,UserIdWhoCreateNovel))
			{
				return 0; // mean user already have novel with this name
			}
			//Add Novel
			var novel = await _AddNovel(novelNameId);

			//Add NovelUser
			await _AddNovelUser(novel.Id, UserIdWhoCreateNovel);
			return novel.Id;
		}

		public async Task<bool> AddNovelUser(int novelId,string username)
		{
			var userId = await _context.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

			if (await IsUserOwnThisNovel(novelId,userId))
			{
				return false; // user already in this novel
			}

			return await _AddNovelUser(novelId, userId);
		}

		public async Task<bool> DeleteNovel(int id)
		{
			//Try Get Novel
			var novelClone = _context.Novels.FirstOrDefault(n => n.Id == id);
			if (novelClone == null)
			{
				return false;
			}

			_context.Novels.Remove(novelClone); // Bycouse Cascade Delete Enabled Remove Novel Will Remove NovelUsers Too
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> IsUserOwnThisNovel(int novelId, int UserId)
		{
			NovelUser? novel = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == UserId);
			return novel != null;
		}
	}
}
