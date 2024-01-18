﻿using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Services;

public class NovelSharedService
{
    private readonly ApplicationDbContext _context;

    public NovelSharedService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserOwnThisNovel(int novelId, int userId)
    {
        var novel = await _context.NovelUsers.FirstOrDefaultAsync(n => n.NovelId == novelId && n.UserId == userId);
        return novel != null;
    }
}