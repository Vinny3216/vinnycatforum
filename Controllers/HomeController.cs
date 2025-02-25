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
    
    // 构造函数
    public HomeController(vinnycatforumContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        // 获取 Discussion 列表，包括 Comment 数量
        var discussions = await _context.Discussion
            .Include(d => d.Comments) // 加载关联的 Comment 列表
            .OrderByDescending(d => d.CreateDate) // 按 CreateDate 倒序排列
            .ToListAsync();

        return View(discussions);
    }

    

    public async Task<IActionResult> Details(int id)
    {
        // 根据 id 获取 Discussion 的详细信息及其评论
        var discussion = await _context.Discussion
            .Include(d => d.Comments) // 加载关联的 Comment 列表
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return RedirectToAction("Details", "Comments", new { id = id });

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
