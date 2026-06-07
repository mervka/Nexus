using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.ViewModels;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;

namespace Nexus.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProfileController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _context = context;
        _environment = environment;
    }
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return NotFound();
        }

        return View(currentUser);
    }
    
    public async Task<IActionResult> Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return NotFound();
        }

        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var profileUser = await _userManager.FindByIdAsync(id);

        if (profileUser == null)
        {
            return NotFound();
        }

        if (currentUser.Id == profileUser.Id)
        {
            return View(profileUser);
        }

        var sharedProjectExists = await _context.Projects
            .AnyAsync(project =>
                project.FounderId == currentUser.Id &&
                project.Members.Any(member => member.UserId == profileUser.Id)
                ||
                project.FounderId == profileUser.Id &&
                project.Members.Any(member => member.UserId == currentUser.Id)
                ||
                project.Members.Any(member => member.UserId == currentUser.Id) &&
                project.Members.Any(member => member.UserId == profileUser.Id)
            );

        if (!sharedProjectExists)
        {
            return Forbid();
        }

        return View(profileUser);
    }

    public async Task<IActionResult> Edit()
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return NotFound();
        }

        var viewModel = new ProfileEditViewModel
        {
            FullName = currentUser.FullName,
            ProfessionalTitle = currentUser.ProfessionalTitle,
            Bio = currentUser.Bio,
            Skills = currentUser.Skills,
            ExperienceLevel = currentUser.ExperienceLevel,
            PortfolioUrl = currentUser.PortfolioUrl,
            GitHubUrl = currentUser.GitHubUrl,
            LinkedInUrl = currentUser.LinkedInUrl,
            IsOpenToCollaboration = currentUser.IsOpenToCollaboration,
            ExistingProfileImagePath = currentUser.ProfileImagePath
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProfileEditViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
        {
            return NotFound();
        }

        currentUser.FullName = viewModel.FullName;
        currentUser.ProfessionalTitle = viewModel.ProfessionalTitle;
        currentUser.Bio = viewModel.Bio;
        currentUser.Skills = viewModel.Skills;
        currentUser.ExperienceLevel = viewModel.ExperienceLevel;
        currentUser.PortfolioUrl = viewModel.PortfolioUrl;
        currentUser.GitHubUrl = viewModel.GitHubUrl;
        currentUser.LinkedInUrl = viewModel.LinkedInUrl;
        currentUser.IsOpenToCollaboration = viewModel.IsOpenToCollaboration;
        
        if (viewModel.ProfileImage != null && viewModel.ProfileImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "profiles"); //wwwroot/uploads/profiles klasörünün fiziksel yolu

            Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(viewModel.ProfileImage.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await viewModel.ProfileImage.CopyToAsync(stream);

            currentUser.ProfileImagePath = $"/uploads/profiles/{fileName}"; //Veritabanına dosyanın yolunu kaydet
        }

        var result = await _userManager.UpdateAsync(currentUser);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(viewModel);
        }

        return RedirectToAction(nameof(Index));
    }
}