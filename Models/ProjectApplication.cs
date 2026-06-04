namespace Nexus.Models;

public class ProjectApplication
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public string ApplicantId { get; set; } = string.Empty;

    public ApplicationUser Applicant { get; set; } = null!;

    public string DesiredRole { get; set; } = string.Empty;

    public string MotivationMessage { get; set; } = string.Empty;

    public string RelevantSkills { get; set; } = string.Empty;

    public string ContributionPlan { get; set; } = string.Empty;

    public string Availability { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
}