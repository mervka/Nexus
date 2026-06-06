using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Models;
using Nexus.ViewModels;

namespace Nexus.Controllers;

[Authorize]
public class TeamsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeamsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

        var foundedProjects = await _context.Projects
            .Where(p => p.FounderId == currentUser.Id)
            .Include(p => p.Founder)
            .Include(p => p.Members)
                .ThenInclude(pm => pm.User)
            .ToListAsync();

        var joinedProjects = await _context.ProjectMembers
            .Where(pm => pm.UserId == currentUser.Id)
            .Include(pm => pm.Project)
            .ThenInclude(p => p.Founder)
            .Include(pm => pm.Project)
            .ThenInclude(p => p.Members)
            .ThenInclude(member => member.User)
            .Select(pm => pm.Project)
            .ToListAsync();

        var allProjects = foundedProjects
            .Concat(joinedProjects)
            .GroupBy(p => p.Id)
            .Select(g => g.First())
            .ToList();

        var viewModel = allProjects.Select(project =>
        {
            var currentUserRole = project.FounderId == currentUser.Id
                ? "Founder"
                : project.Members
                    .Where(m => m.UserId == currentUser.Id)
                    .Select(m => m.RoleInProject)
                    .FirstOrDefault() ?? "Member";

            var members = new List<TeamMemberViewModel>
            {
                new TeamMemberViewModel
                {
                    UserId = project.Founder.Id,
                    FullName = project.Founder.FullName,
                    Email = project.Founder.Email ?? string.Empty,
                    RoleInProject = "Founder",
                    ProfessionalTitle = project.Founder.ProfessionalTitle,
                    Skills = project.Founder.Skills,
                    IsOpenToCollaboration = project.Founder.IsOpenToCollaboration,
                    JoinedAt = project.CreatedAt
                }
            };

            members.AddRange(project.Members.Select(member => new TeamMemberViewModel
            {
                UserId = member.User.Id,
                FullName = member.User.FullName,
                Email = member.User.Email ?? string.Empty,
                RoleInProject = member.RoleInProject,
                ProfessionalTitle = member.User.ProfessionalTitle,
                Skills = member.User.Skills,
                IsOpenToCollaboration = member.User.IsOpenToCollaboration,
                JoinedAt = member.JoinedAt
            }));

            return new TeamsIndexProjectViewModel
            {
                ProjectId = project.Id,
                ProjectTitle = project.Title,
                CurrentUserRole = currentUserRole,
                Members = members
            };
        }).ToList();

        return View(viewModel);
    }
}