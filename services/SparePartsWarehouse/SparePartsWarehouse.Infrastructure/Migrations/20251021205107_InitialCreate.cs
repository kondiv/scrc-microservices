using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparePartsWarehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "spare_parts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    model = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    compatible_equipment_models = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    first_time_delivered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spare_parts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_spare_parts_last_updated_at_id",
                table: "spare_parts",
                columns: new[] { "last_updated_at", "id" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_spare_parts_model",
                table: "spare_parts",
                column: "model");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "spare_parts");
        }
    }
}
