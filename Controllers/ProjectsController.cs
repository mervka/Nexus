using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels;

namespace Nexus.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    public IActionResult Index()
    {
        return Content("Projects page will be created later.");
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var project = new Project
        {
            Title = viewModel.Title,
            ShortDescription = viewModel.ShortDescription,
            Description = viewModel.Description,
            ProblemStatement = viewModel.ProblemStatement,
            TargetAudience = viewModel.TargetAudience,
            Category = viewModel.Category,
            Stage = viewModel.Stage,
            NeededRoles = viewModel.NeededRoles,
            CollaborationType = viewModel.CollaborationType,
            FounderId = currentUser.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = project.Id });
    }

    public IActionResult Details(int id)
    {
        return Content($"Project details page will be created later. Project id: {id}");
    }
}