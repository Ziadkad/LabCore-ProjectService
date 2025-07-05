using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PredecessorTaskIds",
                table: "TaskItem");

            migrationBuilder.AddColumn<int>(
                name: "isCompleted",
                table: "TaskItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "TaskItem");

            migrationBuilder.AddColumn<string>(
                name: "PredecessorTaskIds",
                table: "TaskItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
