using System.ComponentModel.DataAnnotations;

namespace Nexus.ViewModels;

public class ProjectCreateViewModel
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    [Display(Name = "Short Description")]
    public string ShortDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Problem Statement")]
    public string ProblemStatement { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Target Audience")]
    public string TargetAudience { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Stage { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Needed Roles")]
    public string NeededRoles { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Collaboration Type")]
    public string CollaborationType { get; set; } = string.Empty;
}