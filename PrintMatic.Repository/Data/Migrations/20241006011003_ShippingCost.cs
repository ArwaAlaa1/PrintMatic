using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintMatic.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShippingCost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingCostId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippingCosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingCosts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingCostId",
                table: "Orders",
                column: "ShippingCostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingCosts_ShippingCostId",
                table: "Orders",
                column: "ShippingCostId",
                principalTable: "ShippingCosts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingCosts_ShippingCostId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ShippingCosts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingCostId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCostId",
                table: "Orders");
        }
    }
}
