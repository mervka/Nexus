using System.ComponentModel.DataAnnotations;

namespace Nexus.ViewModels;

public class ProfileEditViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Professional Title")]
    public string? ProfessionalTitle { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    [Display(Name = "Skills")]
    public string? Skills { get; set; }

    [Display(Name = "Experience Level")]
    public string? ExperienceLevel { get; set; }

    [Url]
    [Display(Name = "Portfolio URL")]
    public string? PortfolioUrl { get; set; }

    [Url]
    [Display(Name = "GitHub URL")]
    public string? GitHubUrl { get; set; }

    [Url]
    [Display(Name = "LinkedIn URL")]
    public string? LinkedInUrl { get; set; }

    [Display(Name = "Open to Collaboration")]
    public bool IsOpenToCollaboration { get; set; }
}