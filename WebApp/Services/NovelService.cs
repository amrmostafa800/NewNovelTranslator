using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Services
{
    public class NovelService
    {
        private readonly ApplicationDbContext _context;

        public NovelService(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<bool> AddNovelAsync(Novel novel)
        {
            //Check If Novel Exit Or Not
            if (await CheckIsNovelExistByNovelNameAsync(novel.NovelName))
            {
                return false;
            }

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
