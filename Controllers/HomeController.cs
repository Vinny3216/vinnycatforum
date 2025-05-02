using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vinnycatforum.Data;
using vinnycatforum.Models;

namespace vinnycatforum.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly vinnycatforumContext _context;
    
    
    public HomeController(vinnycatforumContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        
        var discussions = await _context.Discussion
            .Include(d => d.Comments) 
            .OrderByDescending(d => d.CreateDate) 
            .ToListAsync();

        return View(discussions);
    }

    

    public async Task<IActionResult> Details(int id)
    {
        
        var discussion = await _context.Discussion
            .Include(d => d.Comments) 
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return RedirectToAction("GetDiscussion", "Home", new { id = id });


    }


    public async Task<IActionResult> GetDiscussion(int id)
    {
        var discussion = await _context.Discussion
            .Include(d => d.Comments)
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View("DiscussionDetail", discussion);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
