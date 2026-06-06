using System.ComponentModel.DataAnnotations;

namespace Nexus.ViewModels;

public class ProjectApplicationCreateViewModel
{
    public int ProjectId { get; set; }

    public string ProjectTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    [Display(Name = "Desired Role")]
    public string DesiredRole { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Display(Name = "Motivation Message")]
    public string MotivationMessage { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    [Display(Name = "Relevant Skills")]
    public string RelevantSkills { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Display(Name = "Contribution Plan")]
    public string ContributionPlan { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Availability { get; set; } = string.Empty;
}