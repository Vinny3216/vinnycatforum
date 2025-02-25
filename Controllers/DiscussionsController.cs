using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vinnycatforum.Data;
using vinnycatforum.Models;

namespace vinnycatforum.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly vinnycatforumContext _context;

        public DiscussionsController(vinnycatforumContext context)
        {
            _context = context;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussion.ToListAsync();

            // 拼接图片路径
            foreach (var discussion in discussions)
            {
                discussion.ImageFilename = Path.Combine("/images", discussion.ImageFilename);
            }


            return View(discussions);
        }

        // GET: Discussions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }
            // 拼接图片路径
            discussion.ImageFilename = Path.Combine("/images", discussion.ImageFilename);
            return View(discussion);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discussions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,ImageFile")] Discussion discussion)
        {
            

                // 自动生成 CreateDate
                discussion.CreateDate = DateTime.Now;

            // 检查并保存图片
            if (discussion.ImageFile != null)
            {
                // 使用 GUID 重命名图片文件，确保唯一性
                discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);

                // 定义保存图片的路径
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);

                // 保存图片到 wwwroot/images 目录
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(stream);
                }
            }


            if (ModelState.IsValid)
            {



                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }

        // POST: Discussions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 保留原有的 CreateDate 和 ImageFilename，不允许用户修改
                    var existingDiscussion = await _context.Discussion.AsNoTracking().FirstOrDefaultAsync(m => m.DiscussionId == id);

                    if (existingDiscussion != null)
                    {
                        discussion.CreateDate = existingDiscussion.CreateDate;
                        discussion.ImageFilename = existingDiscussion.ImageFilename;
                    }
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _context.Discussion
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        // POST: Discussions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussion.FindAsync(id);
            if (discussion != null)
            {
                _context.Discussion.Remove(discussion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussion.Any(e => e.DiscussionId == id);
        }
    }
}
