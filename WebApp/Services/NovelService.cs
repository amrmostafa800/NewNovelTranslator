using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Models;

namespace WebApp.Services
{
    public class NovelService
    {
        private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;


		public NovelService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		public bool IsCurrentUserWhoCreateThisNovel(int novelUserId)
		{
			var currentUserId = _httpContextAccessor!.HttpContext!.User!.FindFirstValue(ClaimTypes.NameIdentifier)!.ToInt()!; // Get Current User ID

			if (currentUserId == novelUserId)
			{
                return true;
			}
            return false;
		}

		public async Task<List<Novel>> GetAllNovelsAsync()
        {
            return await _context.Novels.ToListAsync();
        }

        public async Task<Novel?> GetNovelByIdAsync(int? id)
        {
            var novel = await _context.Novels.FirstOrDefaultAsync(n => n.Id == id);
            if (novel == null)
            {
                return null;
            }
            return novel;
        }

		public async Task<int> GetNovelUserIdByIdAsync(int? id)
		{
			var userId = await _context.Novels
				.Where(n => n.Id == id)
				.Select(n => n.UserId)
				.FirstOrDefaultAsync();

			if (userId == 0) // novel id not found
			{
				return 0;
			}
			return userId;
		}

		public async Task<bool> AddNovelAsync(Novel novel)
        {
            //Check If Novel Exit Or Not
            if (await CheckIsNovelExistByNovelNameAsync(novel.NovelName))
            {
                return false;
            }
			novel.UserId = _httpContextAccessor!.HttpContext!.User!.FindFirstValue(ClaimTypes.NameIdentifier)!.ToInt()!; // Get Current User ID

			_context.Add(novel);
            var changes = await _context.SaveChangesAsync();
            return changes != 0;
        }

        public async Task<bool> CheckIsNovelExistByNovelNameAsync(string novelName)
        {
            return await _context.Novels.AnyAsync(n => n.NovelName == novelName);
        }

        public async Task<bool> CheckIsNovelExistByNovelIdAsync(int id)
        {
            return await _context.Novels.AnyAsync(n => n.Id == id);
        }

        public async Task<bool> UpdateNovelAsync(Novel novel)
        {
            //Check If Novel Not Exit
            if (!await CheckIsNovelExistByNovelIdAsync(novel.Id))
            {
                return false;
            }

            _context.Update(novel);
            var changes = await _context.SaveChangesAsync();
            return changes != 0;
        }

        public async Task<bool> DeleteNovelAsync(int id)
        {
            var novel = await _context.Novels.FindAsync(id);
            if (novel != null)
            {
                _context.Novels.Remove(novel);
            }

            return await _context.SaveChangesAsync() != 0;
        }
    }
}
