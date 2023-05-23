using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configuration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adrress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "dbo",
                columns: table => new
                {
                    CountryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Iso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nicename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Iso3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numcode = table.Column<short>(type: "smallint", nullable: true),
                    Phonecode = table.Column<int>(type: "int", nullable: true),
                    IsSaudi = table.Column<bool>(type: "bit", nullable: false),
                    ExtentionNumber = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                schema: "dbo",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "dbo",
                columns: table => new
                {
                    CityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityNameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryID = table.Column<int>(type: "int", nullable: true),
                    RegionID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryID",
                        column: x => x.CountryID,
                        principalSchema: "dbo",
                        principalTable: "Country",
                        principalColumn: "CountryID");
                    table.ForeignKey(
                        name: "FK_City_Region_RegionID",
                        column: x => x.RegionID,
                        principalSchema: "dbo",
                        principalTable: "Region",
                        principalColumn: "RegionID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryID",
                schema: "dbo",
                table: "City",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_City_IsDeleted",
                schema: "dbo",
                table: "City",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_City_RegionID",
                schema: "dbo",
                table: "City",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsDeleted",
                schema: "dbo",
                table: "Country",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Region_IsDeleted",
                schema: "dbo",
                table: "Region",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Region",
                schema: "dbo");
        }
    }
}
