using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdateNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ImageNews");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTitle",
                table: "ImageNews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionTitle",
                table: "ImageNews");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ImageNews",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
