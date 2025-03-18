using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contributors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    PhoneNumber_CountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber_Number = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber_Extension = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTestReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Specimen_SequentialNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Specimen_Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SpecimenCollectionCenter_Facility = table.Column<string>(type: "TEXT", nullable: false),
                    SpecimenCollectionCenter_TradeName = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_HealthcareProxy = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Animal = table.Column<string>(type: "TEXT", nullable: false),
                    Patient_Category = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Breed = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Age_Years = table.Column<int>(type: "INTEGER", nullable: true),
                    Patient_Age_Months = table.Column<int>(type: "INTEGER", nullable: true),
                    Patient_Age_Days = table.Column<int>(type: "INTEGER", nullable: true),
                    CompleteBloodCountId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributors");

            migrationBuilder.DropTable(
                name: "LabTestReports");
        }
    }
}
