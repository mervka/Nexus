using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        var demoFounder = await CreateUserIfNotExistsAsync(
            userManager,
            email: "founder@nexus.com",
            password: "Founder123!",
            fullName: "Demo Founder",
            title: "Product Builder"
        );

        await CreateUserIfNotExistsAsync(
            userManager,
            email: "expert@nexus.com",
            password: "Expert123!",
            fullName: "Demo Expert",
            title: "Full Stack Developer"
        );

        if (await context.Projects.AnyAsync(p => p.Title == "EduPath AI"))
        {
            return;
        }

        var projects = new List<Project>
        {
            new Project
            {
                Title = "EduPath AI",
                ShortDescription = "An AI-supported learning roadmap platform for university students.",
                Description = "EduPath AI helps students create personalized learning paths based on their goals, courses, and current skill level.",
                ProblemStatement = "Students often struggle to decide what to learn next and how to organize their study plan.",
                TargetAudience = "University students and self-learners",
                Category = "Education",
                Stage = "Idea",
                NeededRoles = "Backend Developer, UI Designer, AI Researcher",
                CollaborationType = "Remote",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                FounderId = demoFounder.Id
            },
            new Project
            {
                Title = "GreenTask",
                ShortDescription = "A simple task management app focused on sustainable habits.",
                Description = "GreenTask combines daily task tracking with eco-friendly habit suggestions to help users build a more sustainable lifestyle.",
                ProblemStatement = "Many people want to live more sustainably but do not know how to turn this goal into small daily actions.",
                TargetAudience = "Young adults and environmentally conscious users",
                Category = "Sustainability",
                Stage = "Prototype",
                NeededRoles = "Mobile Developer, UX Designer, Content Writer",
                CollaborationType = "Hybrid",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                FounderId = demoFounder.Id
            },
            new Project
            {
                Title = "HealthBridge",
                ShortDescription = "A platform that helps patients prepare better questions before doctor visits.",
                Description = "HealthBridge allows users to organize symptoms, concerns, and questions before medical appointments.",
                ProblemStatement = "Patients often forget important details during short doctor visits.",
                TargetAudience = "Patients and caregivers",
                Category = "Health",
                Stage = "MVP",
                NeededRoles = "Frontend Developer, Healthcare Advisor",
                CollaborationType = "Remote",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddMinutes(-20),
                FounderId = demoFounder.Id
            }
        };

        context.Projects.AddRange(projects);
        await context.SaveChangesAsync();
    }

    private static async Task<ApplicationUser> CreateUserIfNotExistsAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        string title)
    {
        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            return existingUser;
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = fullName,
            ProfessionalTitle = title,
            Bio = "This is a demo profile created for the NEXUS MVP.",
            Skills = "ASP.NET Core, MVC, Collaboration",
            ExperienceLevel = "Intermediate",
            IsOpenToCollaboration = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Demo user could not be created: {errors}");
        }

        return user;
    }
}