using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexus.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueProjectConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectApplications_ProjectId",
                table: "ProjectApplications");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId_UserId",
                table: "ProjectMembers",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectApplications_ProjectId_ApplicantId",
                table: "ProjectApplications",
                columns: new[] { "ProjectId", "ApplicantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectId_UserId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectApplications_ProjectId_ApplicantId",
                table: "ProjectApplications");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectApplications_ProjectId",
                table: "ProjectApplications",
                column: "ProjectId");
        }
    }
}
