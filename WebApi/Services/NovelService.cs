using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Services;

public class NovelService
{
    private readonly ApplicationDbContext _context;
    private readonly NovelUserService _novelUserService;

    public NovelService(ApplicationDbContext context, NovelUserService novelUserService)
    {
        _context = context;
        _novelUserService = novelUserService;
    }

    public List<NovelDto> GetAllNovels()
    {
        var linqQuery = from novel in _context.Novels
            join novelUser in _context.NovelUsers on novel.Id equals novelUser.NovelId
            where novelUser.IsOwner == true
            select new NovelDto
            {
                Id = novel.Id,
                UserId = novelUser.UserId,
                UserName = novelUser.User.UserName,
                Name = novel.NovelName
            };
        
        return linqQuery.ToList();
    }

    public NovelDto? GetById(int id)
    {
        return _context.NovelUsers.Where(n => n.NovelId == id).Select(n => new NovelDto
        {
            Id = n.NovelId,
            UserId = n.UserId,
            UserName = n.User!.UserName!,
            Name = n.Novel!.NovelName!
        }).FirstOrDefault();
    }

    private async Task<Novel> _AddNovel(string novelName)
    {
        var novel = new Novel
        {
            NovelName = novelName
        };

        await _context.Novels.AddAsync(novel);

        await _context.SaveChangesAsync();

        return novel; // check if Added Successfully
    }

    private bool _CheckIfNovelAlreadyExistWithThisUser(string novelName, int userId)
    {
        if (_context.NovelUsers.Any(n => n.Novel!.NovelName == novelName && n.UserId == userId))
        {
            return true;
        }
        
        return false;
    }

    public async Task<int> AddNovel(string novelName, int userIdWhoCreateNovel)
    {

        //Check If Novel Is Exist (this user already have novel with this name)
        if (_CheckIfNovelAlreadyExistWithThisUser(novelName, userIdWhoCreateNovel))
        {
            return 0; // mean user already have novel with this name
        } 
        //Add Novel
        var novel = await _AddNovel(novelName);

        //Add NovelUser as Owner
        await _novelUserService.AddNovelUser(novel.Id, userIdWhoCreateNovel,true);
        return novel.Id;
    }

    public async Task<bool> DeleteNovel(int id)
    {
        //Try Get Novel
        var novel = _context.Novels.FirstOrDefault(n => n.Id == id);
        
        if (novel == null) 
            return false;

        _context.Novels.Remove(novel); // Bycouse Cascade Delete Enabled Remove Novel Will Remove NovelUsers Too
        await _context.SaveChangesAsync();
        return true;
    }
}