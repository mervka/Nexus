namespace Nexus.ViewModels;

public class TeamsIndexProjectViewModel
{
    public int ProjectId { get; set; }

    public string ProjectTitle { get; set; } = string.Empty;

    public string CurrentUserRole { get; set; } = string.Empty;

    public List<TeamMemberViewModel> Members { get; set; } = new();
}