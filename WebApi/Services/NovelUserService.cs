using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Services;

public class NovelUserService
{
    private readonly ApplicationDbContext _context;
    private readonly NovelSharedService _novelSharedService;

    public NovelUserService(ApplicationDbContext context, NovelSharedService novelSharedService)
    {
        _context = context;
        _novelSharedService = novelSharedService;
    }
    
    public async Task<bool> AddNovelUser(int novelId, string username)
    {
        var userId = await _context.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

        if (await _novelSharedService.IsUserOwnThisNovel(novelId, userId)) 
            return false; // user already in this novel

        return await AddNovelUser(novelId, userId);
    }
    
    public async Task<bool> AddNovelUser(int novelId, int userId)
    {
        var novelUser = new NovelUser
        {
            NovelId = novelId,
            UserId = userId
        };

        await _context.NovelUsers.AddAsync(novelUser);

        return await _context.SaveChangesAsync() != 0; // check if Added Successfully
    }
    

}