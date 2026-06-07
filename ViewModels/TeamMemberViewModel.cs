namespace Nexus.ViewModels;

public class TeamMemberViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string RoleInProject { get; set; } = string.Empty;

    public string? ProfessionalTitle { get; set; }

    public string? Skills { get; set; }

    public bool IsOpenToCollaboration { get; set; }

    public DateTime JoinedAt { get; set; }
    
    public string? ProfileImagePath { get; set; }
}