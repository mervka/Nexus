using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.ViewModels;
using Microsoft.AspNetCore.Identity;
using Nexus.Models;
using Microsoft.AspNetCore.Authorization;

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
    
    [Authorize]
    public async Task<IActionResult> Apply(int id) //Formu açmak için 
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (project == null)
        {
            return NotFound();
        }

        if (project.FounderId == currentUser.Id)
        {
            return BadRequest("You cannot apply to your own project.");
        }

        var alreadyApplied = await _context.ProjectApplications
            .AnyAsync(pa => pa.ProjectId == id && pa.ApplicantId == currentUser.Id);

        if (alreadyApplied)
        {
            return BadRequest("You have already applied to this project.");
        }

        var alreadyMember = await _context.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == id && pm.UserId == currentUser.Id);

        if (alreadyMember)
        {
            return BadRequest("You are already a member of this project.");
        }

        var viewModel = new ProjectApplicationCreateViewModel
        {
            ProjectId = project.Id,
            ProjectTitle = project.Title
        };

        return View(viewModel);
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(ProjectApplicationCreateViewModel viewModel) //Formu doldurmak için
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

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == viewModel.ProjectId && p.IsActive);

        if (project == null)
        {
            return NotFound();
        }

        if (project.FounderId == currentUser.Id)
        {
            return BadRequest("You cannot apply to your own project.");
        }

        var alreadyApplied = await _context.ProjectApplications
            .AnyAsync(pa => pa.ProjectId == viewModel.ProjectId && pa.ApplicantId == currentUser.Id);

        if (alreadyApplied)
        {
            return BadRequest("You have already applied to this project.");
        }

        var alreadyMember = await _context.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == viewModel.ProjectId && pm.UserId == currentUser.Id);

        if (alreadyMember)
        {
            return BadRequest("You are already a member of this project.");
        }

        var application = new ProjectApplication
        {
            ProjectId = viewModel.ProjectId,
            ApplicantId = currentUser.Id,
            DesiredRole = viewModel.DesiredRole,
            MotivationMessage = viewModel.MotivationMessage,
            RelevantSkills = viewModel.RelevantSkills,
            ContributionPlan = viewModel.ContributionPlan,
            Availability = viewModel.Availability,
            Status = "Pending",
            AppliedAt = DateTime.UtcNow
        };

        _context.ProjectApplications.Add(application);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = viewModel.ProjectId });
    }
}