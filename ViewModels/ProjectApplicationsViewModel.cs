namespace Nexus.ViewModels;

public class ProjectApplicationsViewModel
{
    public int ProjectId { get; set; }

    public string ProjectTitle { get; set; } = string.Empty;

    public List<ProjectApplicationListItemViewModel> Applications { get; set; } = new();
}