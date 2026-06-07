using Microsoft.AspNetCore.Identity;

namespace Nexus.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public string? ProfessionalTitle { get; set; }

    public string? Bio { get; set; }

    public string? Skills { get; set; }

    public string? ExperienceLevel { get; set; }

    public string? PortfolioUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public string? LinkedInUrl { get; set; }
    
    public string? ProfileImagePath { get; set; }

    public bool IsOpenToCollaboration { get; set; }
    
    public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();

    public ICollection<ProjectApplication> Applications { get; set; } = new List<ProjectApplication>();

    public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
    
}