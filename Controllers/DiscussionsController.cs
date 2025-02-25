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
            

                // CreateDate
                discussion.CreateDate = DateTime.Now;

            
            if (discussion.ImageFile != null)
            {
                
                discussion.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(discussion.ImageFile.FileName);

                
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", discussion.ImageFilename);

                
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
