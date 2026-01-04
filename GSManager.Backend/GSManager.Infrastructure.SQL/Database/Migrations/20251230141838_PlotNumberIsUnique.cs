using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GSManager.Infrastructure.SQL.Database.Migrations;

/// <inheritdoc />
public partial class PlotNumberIsUnique : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Number",
            table: "Plots",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Plots_Number",
            table: "Plots",
            column: "Number",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Plots_Number",
            table: "Plots");

        migrationBuilder.AlterColumn<string>(
            name: "Number",
            table: "Plots",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");
    }
}
