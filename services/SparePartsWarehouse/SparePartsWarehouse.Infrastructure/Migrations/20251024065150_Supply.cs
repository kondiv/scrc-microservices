using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparePartsWarehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Supply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_spare_parts_model",
                table: "spare_parts");

            migrationBuilder.CreateTable(
                name: "supply",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    arrived_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supply", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "supply_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    model = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    compatible_equipment_models = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    supply_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supply_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_supply_item_supply_supply_id",
                        column: x => x.supply_id,
                        principalTable: "supply",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_spare_parts_model",
                table: "spare_parts",
                column: "model",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supply_item_supply_id",
                table: "supply_item",
                column: "supply_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "supply_item");

            migrationBuilder.DropTable(
                name: "supply");

            migrationBuilder.DropIndex(
                name: "IX_spare_parts_model",
                table: "spare_parts");

            migrationBuilder.CreateIndex(
                name: "IX_spare_parts_model",
                table: "spare_parts",
                column: "model");
        }
    }
}
