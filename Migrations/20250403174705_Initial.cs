using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDR_Analytics.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CDRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallerID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Recipient = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CallDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CDRs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Idx_CallDate",
                table: "CDRs",
                column: "CallDate");

            migrationBuilder.CreateIndex(
                name: "Idx_CallerID",
                table: "CDRs",
                column: "CallerID");

            migrationBuilder.CreateIndex(
                name: "Idx_Recipient",
                table: "CDRs",
                column: "Recipient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CDRs");
        }
    }
}
