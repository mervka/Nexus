namespace Nexus.ViewModels;

public class ProjectListItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Stage { get; set; } = string.Empty;

    public string RoleInProject { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}