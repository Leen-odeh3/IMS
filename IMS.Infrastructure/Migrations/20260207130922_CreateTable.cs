using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "university",
                table: "AspNetUsers",
                newName: "University");

            migrationBuilder.RenameColumn(
                name: "specialization",
                table: "AspNetUsers",
                newName: "Specialization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "University",
                table: "AspNetUsers",
                newName: "university");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "AspNetUsers",
                newName: "specialization");
        }
    }
}
