using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.ViewModels;
using Microsoft.AspNetCore.Identity;
using Nexus.Models;

namespace Nexus.Controllers;

public class ExploreController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ExploreController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        var projectsQuery = _context.Projects
            .Where(p => p.IsActive);

        if (currentUser != null)
        {
            projectsQuery = projectsQuery
                .Where(p => p.FounderId != currentUser.Id);
        }

        var projects = await projectsQuery
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProjectListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ShortDescription = p.ShortDescription,
                Category = p.Category,
                Stage = p.Stage,
                RoleInProject = "Open Project",
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        return View(projects);
    }

    public async Task<IActionResult> Details(int id)
    {
        var project = await _context.Projects
            .Include(p => p.Founder)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }
}