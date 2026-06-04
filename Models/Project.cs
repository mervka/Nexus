namespace Nexus.Models;

public class Project
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ProblemStatement { get; set; } = string.Empty;

    public string TargetAudience { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Stage { get; set; } = string.Empty;

    public string NeededRoles { get; set; } = string.Empty;

    public string CollaborationType { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FounderId { get; set; } = string.Empty;

    public ApplicationUser Founder { get; set; } = null!;

    public ICollection<ProjectApplication> Applications { get; set; } = new List<ProjectApplication>();

    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
}