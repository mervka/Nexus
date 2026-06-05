using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectApplication> ProjectApplications { get; set; }

    public DbSet<ProjectMember> ProjectMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .HasOne(p => p.Founder)
            .WithMany(u => u.OwnedProjects)
            .HasForeignKey(p => p.FounderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProjectApplication>()
            .HasOne(pa => pa.Project)
            .WithMany(p => p.Applications)
            .HasForeignKey(pa => pa.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectApplication>()
            .HasOne(pa => pa.Applicant)
            .WithMany(u => u.Applications)
            .HasForeignKey(pa => pa.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProjectMember>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.Members)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProjectMember>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMemberships)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}