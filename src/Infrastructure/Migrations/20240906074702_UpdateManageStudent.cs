using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateManageStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HighSchoolId",
                table: "students",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desciption = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "highschols",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoldBalance = table.Column<int>(type: "int", nullable: false),
                    LocationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactInfor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_highschols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_highschols_regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_HighSchoolId",
                table: "students",
                column: "HighSchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_highschols_RegionId",
                table: "highschols",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_students_highschols_HighSchoolId",
                table: "students",
                column: "HighSchoolId",
                principalTable: "highschols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_highschols_HighSchoolId",
                table: "students");

            migrationBuilder.DropTable(
                name: "highschols");

            migrationBuilder.DropTable(
                name: "regions");

            migrationBuilder.DropIndex(
                name: "IX_students_HighSchoolId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "HighSchoolId",
                table: "students");
        }
    }
}
