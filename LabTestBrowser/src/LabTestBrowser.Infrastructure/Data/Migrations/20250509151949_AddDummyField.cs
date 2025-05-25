﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDummyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Patient_Age_IsEmpty",
                table: "LabTestReports",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Patient_Age_IsEmpty",
                table: "LabTestReports");
        }
    }
}
