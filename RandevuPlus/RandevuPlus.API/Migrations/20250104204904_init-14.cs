using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandevuPlus.API.Migrations
{
    /// <inheritdoc />
    public partial class init14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorExperience_Instructors_InstructorId",
                table: "InstructorExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSkill_Instructors_InstructorId",
                table: "InstructorSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorSkill",
                table: "InstructorSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorExperience",
                table: "InstructorExperience");

            migrationBuilder.RenameTable(
                name: "InstructorSkill",
                newName: "InstructorSkills");

            migrationBuilder.RenameTable(
                name: "InstructorExperience",
                newName: "InstructorExperiences");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorSkill_InstructorId",
                table: "InstructorSkills",
                newName: "IX_InstructorSkills_InstructorId");

            migrationBuilder.RenameColumn(
                name: "ExperienceDescription",
                table: "InstructorExperiences",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorExperience_InstructorId",
                table: "InstructorExperiences",
                newName: "IX_InstructorExperiences_InstructorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorSkills",
                table: "InstructorSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorExperiences",
                table: "InstructorExperiences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorExperiences_Instructors_InstructorId",
                table: "InstructorExperiences",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSkills_Instructors_InstructorId",
                table: "InstructorSkills",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorExperiences_Instructors_InstructorId",
                table: "InstructorExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSkills_Instructors_InstructorId",
                table: "InstructorSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorSkills",
                table: "InstructorSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstructorExperiences",
                table: "InstructorExperiences");

            migrationBuilder.RenameTable(
                name: "InstructorSkills",
                newName: "InstructorSkill");

            migrationBuilder.RenameTable(
                name: "InstructorExperiences",
                newName: "InstructorExperience");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorSkills_InstructorId",
                table: "InstructorSkill",
                newName: "IX_InstructorSkill_InstructorId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "InstructorExperience",
                newName: "ExperienceDescription");

            migrationBuilder.RenameIndex(
                name: "IX_InstructorExperiences_InstructorId",
                table: "InstructorExperience",
                newName: "IX_InstructorExperience_InstructorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorSkill",
                table: "InstructorSkill",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InstructorExperience",
                table: "InstructorExperience",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorExperience_Instructors_InstructorId",
                table: "InstructorExperience",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSkill_Instructors_InstructorId",
                table: "InstructorSkill",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
