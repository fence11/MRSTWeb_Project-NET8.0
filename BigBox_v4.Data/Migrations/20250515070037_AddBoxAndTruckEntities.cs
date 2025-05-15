using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBox_v4.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBoxAndTruckEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoxSizes",
                columns: table => new
                {
                    BoxSizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Width = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxSizes", x => x.BoxSizeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    TruckID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TruckBrand = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TowWidth = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TowHeight = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TowLength = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TruckState = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LicensePlate = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManufactureYear = table.Column<int>(type: "int", nullable: true),
                    MaxLoadCapacity = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.TruckID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    BoxId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Width = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ThisWayUp = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Fragile = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HandleWithCare = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    KeepDry = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    KeepUpright = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Perishable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DoNotStack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Flammable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Explosive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DoNotOpenBeforeDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AdditionalNotes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriverId = table.Column<int>(type: "int", nullable: true),
                    TruckId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.BoxId);
                    table.ForeignKey(
                        name: "FK_Boxes_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boxes_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "TruckID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_DriverId",
                table: "Boxes",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_TruckId",
                table: "Boxes",
                column: "TruckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "BoxSizes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Trucks");
        }
    }
}
