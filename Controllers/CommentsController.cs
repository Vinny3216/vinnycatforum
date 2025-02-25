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
    public class CommentsController : Controller
    {
        private readonly vinnycatforumContext _context;

        public CommentsController(vinnycatforumContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comment.ToListAsync();

            
            foreach (var comment in comments)
            {
                comment.ImageFilename = Path.Combine("/images", comment.ImageFilename);
            }

            return View(comments);
        }


        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Discussion)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            
            comment.ImageFilename = Path.Combine("/images", comment.ImageFilename);
            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["DiscussionId"] = new SelectList(_context.Set<Discussion>(), "DiscussionId", "Content");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId,Content,DiscussionId,ImageFile")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // CreateDate
                comment.CreateDate = DateTime.Now;

                
                if (comment.ImageFile != null)
                {
                    
                    comment.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(comment.ImageFile.FileName);

                    
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", comment.ImageFilename);

                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await comment.ImageFile.CopyToAsync(stream);
                    }
                }

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscussionId"] = new SelectList(_context.Set<Discussion>(), "DiscussionId", "Content", comment.DiscussionId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["DiscussionId"] = new SelectList(_context.Set<Discussion>(), "DiscussionId", "Content", comment.DiscussionId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Content,CreateDate,DiscussionId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var existingComment = await _context.Comment
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.CommentId == id);

                    if (existingComment != null)
                    {
                        comment.CreateDate = existingComment.CreateDate;
                    }
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["DiscussionId"] = new SelectList(_context.Set<Discussion>(), "DiscussionId", "Content", comment.DiscussionId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Discussion)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.CommentId == id);
        }
    }
}
