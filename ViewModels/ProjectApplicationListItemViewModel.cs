namespace Nexus.ViewModels;

public class ProjectApplicationListItemViewModel
{
    public int Id { get; set; }

    public string ApplicantName { get; set; } = string.Empty;

    public string ApplicantEmail { get; set; } = string.Empty;

    public string DesiredRole { get; set; } = string.Empty;

    public string MotivationMessage { get; set; } = string.Empty;

    public string RelevantSkills { get; set; } = string.Empty;

    public string ContributionPlan { get; set; } = string.Empty;

    public string Availability { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime AppliedAt { get; set; }
}