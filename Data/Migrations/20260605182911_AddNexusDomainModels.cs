using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNexusDomainModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceLevel",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenToCollaboration",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioUrl",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalTitle",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ProblemStatement = table.Column<string>(type: "TEXT", nullable: false),
                    TargetAudience = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Stage = table.Column<string>(type: "TEXT", nullable: false),
                    NeededRoles = table.Column<string>(type: "TEXT", nullable: false),
                    CollaborationType = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FounderId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_FounderId",
                        column: x => x.FounderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicantId = table.Column<string>(type: "TEXT", nullable: false),
                    DesiredRole = table.Column<string>(type: "TEXT", nullable: false),
                    MotivationMessage = table.Column<string>(type: "TEXT", nullable: false),
                    RelevantSkills = table.Column<string>(type: "TEXT", nullable: false),
                    ContributionPlan = table.Column<string>(type: "TEXT", nullable: false),
                    Availability = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectApplications_AspNetUsers_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectApplications_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleInProject = table.Column<string>(type: "TEXT", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectApplications_ApplicantId",
                table: "ProjectApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectApplications_ProjectId",
                table: "ProjectApplications",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_FounderId",
                table: "Projects",
                column: "FounderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectApplications");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsOpenToCollaboration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PortfolioUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfessionalTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "AspNetUsers");
        }
    }
}
