using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class NovelsController : Controller //Controller Auto Generated And I Edit It
    {
        private readonly NovelService _novelService;

        public NovelsController(NovelService novelService)
        {
            this._novelService = novelService;
        }

        // GET: Novels
        public async Task<IActionResult> Index()
        {
            var novels = await _novelService.GetAllNovelsAsync();
            return View(novels);
        }

        // GET: Novels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novel = await _novelService.GetNovelByIdAsync(id);
            if (novel == null)
            {
                return NotFound();
            }

            return View(novel);
        }

        // GET: Novels/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Novels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Create([Bind("NovelName")] Novel novel)
        {
			if (ModelState.IsValid)
            {
                if (await _novelService.AddNovelAsync(novel))
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest("Novel Already Exist");
            }
            return View(novel);
        }

		// GET: Novels/Edit/5
		[Authorize]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var novel = await _novelService.GetNovelByIdAsync(id);
            if (novel == null)
            {
                return NotFound();
            }

            if (!_novelService.IsCurrentUserWhoCreateThisNovel(novel.UserId)) 
            {
				return NotFound();
			}

			return View(novel);
        }

        // POST: Novels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> Edit(int id, [Bind("Id,NovelName")] Novel novel)
        {
            if (id != novel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				var novelUserId = await _novelService.GetNovelUserIdByIdAsync(id); // if novel id not found will get 0
				novel.UserId = novelUserId; //Bind Will Make it 0 alwayes So Need Edit Here
				if (!_novelService.IsCurrentUserWhoCreateThisNovel(novelUserId))
				{
					return NotFound();
				}

				try
                {
					await _novelService.UpdateNovelAsync(novel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(novel);
        }

		// GET: Novels/Delete/5
		[Authorize]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var novel = await _novelService.GetNovelByIdAsync(id);
            if (novel == null || !_novelService.IsCurrentUserWhoCreateThisNovel(novel.UserId))
            {
                return NotFound();
            }

			return View(novel);
        }

        // POST: Novels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var novelUserId = await _novelService.GetNovelUserIdByIdAsync(id); // if novel id not found will get 0
			if (!_novelService.IsCurrentUserWhoCreateThisNovel(novelUserId))
			{
				return NotFound();
			}

			if (await _novelService.DeleteNovelAsync(id))
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("Novel Id Not Exist");
        }
    }
}
