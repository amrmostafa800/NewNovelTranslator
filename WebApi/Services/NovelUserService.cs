using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Enums;
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
            NovelUserId = novelUser.Id,
            NovelId = novelUser.NovelId,
        }).ToArray();
    }

    public async Task<NovelUser?> GetNovelUser(int novelUserId)
    {
        return await _context.NovelUsers.FirstOrDefaultAsync(n => n.Id == novelUserId);
    }
    
    public async Task<NovelUser?> GetNovelUserOwnerByNovelId(int novelId)
    {
        return await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.IsOwner == true);
    }
    
    public async Task<Tuple<EAddNovelUserResult,int>> AddNovelUser(int novelId, string username,bool isOwner = false)
    {
        var userId = await _context.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

        if (userId == 0)
        {
            return new Tuple<EAddNovelUserResult, int>(EAddNovelUserResult.UsernameNotExist,0);
        }
        
        return await AddNovelUser(novelId, userId);
    }
    
    public async Task<Tuple<EAddNovelUserResult,int>> AddNovelUser(int novelId, int userId, bool isOwner = false)
    {
        if (await _novelSharedService.IsUserHavePermissionOnThisNovel(novelId, userId)) 
            return new Tuple<EAddNovelUserResult, int>(EAddNovelUserResult.AlreadyOwnPermission,0); // user already Have Permission in this novel
        
        var novelUser = new NovelUser
        {
            NovelId = novelId,
            UserId = userId,
            IsOwner = isOwner
        };

        await _context.NovelUsers.AddAsync(novelUser);

        if (await _context.SaveChangesAsync() != 0)
        {
            return new Tuple<EAddNovelUserResult, int>(EAddNovelUserResult.Success,novelUser.Id);
        }
        
        return new Tuple<EAddNovelUserResult, int>(EAddNovelUserResult.UnknownError,0);
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
        var novelUser = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == userId);

        if (novelUser is null)
            return false;
        
        _context.NovelUsers.Remove(novelUser);

        return await _context.SaveChangesAsync() != 0; // check if Removed Successfully
    }
    
    public async Task<ERemoveNovelUserResult> RemoveNovelUserByNovelUserId(int novelUserId,int userId)
    {
        var novelUser = await _context.NovelUsers.FirstOrDefaultAsync(n => n.Id == novelUserId);

        if (novelUser is null)
            return ERemoveNovelUserResult.ThisNovelUserIdNotExist;

        if (novelUser.UserId == userId || novelUser.IsOwner) // Mean Owner Of Novel Try Remove Itself
        {
            return ERemoveNovelUserResult.OwnerTryRemoveItself;
        }
        
        _context.NovelUsers.Remove(novelUser);

        if (await _context.SaveChangesAsync() == 0)
        {
            return ERemoveNovelUserResult.AlreadyDontOwnPermission;
        }

        return ERemoveNovelUserResult.Success;
    }
}