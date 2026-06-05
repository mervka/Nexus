using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.ViewModels;

namespace Nexus.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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
            IsOpenToCollaboration = currentUser.IsOpenToCollaboration
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