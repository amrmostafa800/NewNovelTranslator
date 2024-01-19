using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services;

public class NovelSharedService
{
    private readonly ApplicationDbContext _context;

    public NovelSharedService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserHavePermissionOnThisNovel(int novelId, int userId)
    {
        var novel = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == userId);
        return novel != null;
    }
    
    public async Task<bool> IsUserHavePermissionOnThisNovelByNovelUserId(int novelUserId, int userId)
    {
        var novel = await _context.NovelUsers.FirstOrDefaultAsync(n => n.Id == novelUserId && n.UserId == userId);
        return novel != null;
    }
}