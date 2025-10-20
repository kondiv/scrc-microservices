using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EquipmentService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnneccesaryProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requires_special_training",
                table: "equipment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "requires_special_training",
                table: "equipment",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
