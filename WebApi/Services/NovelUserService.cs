using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
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

    public NovelUserDto[] GetNovelUsersByNovelId(int novelId)
    {
        return _context.NovelUsers.Where(n => n.NovelId == novelId).Select(novelUser => new NovelUserDto()
        {
            UserName = novelUser.User!.UserName!,
            NovelId = novelUser.NovelId
        }).ToArray();
    }
    
    public async Task<bool> AddNovelUser(int novelId, string username)
    {
        var userId = await _context.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

        if (userId == 0)
        {
            return false;
        }
        
        return await AddNovelUser(novelId, userId);
    }
    
    public async Task<bool> AddNovelUser(int novelId, int userId)
    {
        if (await _novelSharedService.IsUserHavePermissionOnThisNovel(novelId, userId)) 
            return false; // user already Have Permission in this novel
        
        var novelUser = new NovelUser
        {
            NovelId = novelId,
            UserId = userId
        };

        await _context.NovelUsers.AddAsync(novelUser);

        return await _context.SaveChangesAsync() != 0; // check if Added Successfully
    }
    
    public async Task<bool> RemoveNovelUser(int novelId, string username)
    {
        var userId = await _context.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

        if (userId == 0)
        {
            return false;
        }
        
        return await RemoveNovelUser(novelId, userId);
    }
    
    public async Task<bool> RemoveNovelUser(int novelId, int userId)
    {
        if (!await _novelSharedService.IsUserHavePermissionOnThisNovel(novelId, userId)) 
            return false; // user already Dont Have Permission in this novel

        var novelUser = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == userId);

        if (novelUser is null)
            return false;
        
        _context.NovelUsers.Remove(novelUser);

        return await _context.SaveChangesAsync() != 0; // check if Removed Successfully
    }
}