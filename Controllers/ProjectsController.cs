using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var ownedProjects = await _context.Projects
            .Where(p => p.FounderId == currentUser.Id)
            .Select(p => new ProjectListItemViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ShortDescription = p.ShortDescription,
                Category = p.Category,
                Stage = p.Stage,
                RoleInProject = "Founder",
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        var joinedProjects = await _context.ProjectMembers
            .Where(pm => pm.UserId == currentUser.Id)
            .Select(pm => new ProjectListItemViewModel
            {
                Id = pm.Project.Id,
                Title = pm.Project.Title,
                ShortDescription = pm.Project.ShortDescription,
                Category = pm.Project.Category,
                Stage = pm.Project.Stage,
                RoleInProject = pm.RoleInProject,
                IsActive = pm.Project.IsActive,
                CreatedAt = pm.Project.CreatedAt
            })
            .ToListAsync();

        var allProjects = ownedProjects
            .Concat(joinedProjects)
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .OrderByDescending(p => p.CreatedAt)
            .ToList();

        return View(allProjects);
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

    public async Task<IActionResult> Details(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var project = await _context.Projects
            .Include(p => p.Founder)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        string? userRole = null;

        if (project.FounderId == currentUser.Id)
        {
            userRole = "Founder";
        }
        else
        {
            userRole = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == project.Id && pm.UserId == currentUser.Id)
                .Select(pm => pm.RoleInProject)
                .FirstOrDefaultAsync();
        }

        if (userRole == null)
        {
            return Forbid();
        }

        ViewBag.UserRole = userRole;

        return View(project);
    }
    
    public async Task<IActionResult> Edit(int id)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
    {
        return Unauthorized();
    }

    var project = await _context.Projects
        .FirstOrDefaultAsync(p => p.Id == id);

    if (project == null)
    {
        return NotFound();
    }

    if (project.FounderId != currentUser.Id)
    {
        return Forbid();
    }

    var viewModel = new ProjectEditViewModel
    {
        Id = project.Id,
        Title = project.Title,
        ShortDescription = project.ShortDescription,
        Description = project.Description,
        ProblemStatement = project.ProblemStatement,
        TargetAudience = project.TargetAudience,
        Category = project.Category,
        Stage = project.Stage,
        NeededRoles = project.NeededRoles,
        CollaborationType = project.CollaborationType,
        IsActive = project.IsActive
    };

    return View(viewModel);
}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProjectEditViewModel viewModel)
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
            .FirstOrDefaultAsync(p => p.Id == viewModel.Id);

        if (project == null)
        {
            return NotFound();
        }

        if (project.FounderId != currentUser.Id)
        {
            return Forbid();
        }

        project.Title = viewModel.Title;
        project.ShortDescription = viewModel.ShortDescription;
        project.Description = viewModel.Description;
        project.ProblemStatement = viewModel.ProblemStatement;
        project.TargetAudience = viewModel.TargetAudience;
        project.Category = viewModel.Category;
        project.Stage = viewModel.Stage;
        project.NeededRoles = viewModel.NeededRoles;
        project.CollaborationType = viewModel.CollaborationType;
        project.IsActive = viewModel.IsActive;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = project.Id });
    }
    
    public async Task<IActionResult> Applications(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        if (project.FounderId != currentUser.Id)
        {
            return Forbid();
        }

        var applications = await _context.ProjectApplications
            .Include(a => a.Applicant)
            .Where(a => a.ProjectId == id)
            .OrderByDescending(a => a.AppliedAt)
            .Select(a => new ProjectApplicationListItemViewModel
            {
                Id = a.Id,
                ApplicantName = a.Applicant.FullName,
                ApplicantEmail = a.Applicant.Email ?? string.Empty,
                DesiredRole = a.DesiredRole,
                MotivationMessage = a.MotivationMessage,
                RelevantSkills = a.RelevantSkills,
                ContributionPlan = a.ContributionPlan,
                Availability = a.Availability,
                Status = a.Status,
                AppliedAt = a.AppliedAt
            })
            .ToListAsync();

        var viewModel = new ProjectApplicationsViewModel
        {
            ProjectId = project.Id,
            ProjectTitle = project.Title,
            Applications = applications
        };

        return View(viewModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptApplication(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var application = await _context.ProjectApplications
            .Include(a => a.Project)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Project.FounderId != currentUser.Id)
        {
            return Forbid();
        }

        if (application.Status != "Pending")
        {
            return RedirectToAction(nameof(Applications), new { id = application.ProjectId });
        }

        var alreadyMember = await _context.ProjectMembers
            .AnyAsync(pm => pm.ProjectId == application.ProjectId && pm.UserId == application.ApplicantId);

        if (!alreadyMember)
        {
            var projectMember = new ProjectMember
            {
                ProjectId = application.ProjectId,
                UserId = application.ApplicantId,
                RoleInProject = application.DesiredRole,
                JoinedAt = DateTime.UtcNow
            };

            _context.ProjectMembers.Add(projectMember);
        }

        application.Status = "Accepted";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Applications), new { id = application.ProjectId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectApplication(int id)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var application = await _context.ProjectApplications
            .Include(a => a.Project)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        if (application.Project.FounderId != currentUser.Id)
        {
            return Forbid();
        }

        if (application.Status != "Pending")
        {
            return RedirectToAction(nameof(Applications), new { id = application.ProjectId });
        }

        application.Status = "Rejected";

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Applications), new { id = application.ProjectId });
    }
}