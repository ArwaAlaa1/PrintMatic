using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrintMatic.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_AddressDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductItem_ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductItem_Photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_NormalMinDate = table.Column<int>(type: "int", nullable: true),
                    ProductItem_NormalMaxDate = table.Column<int>(type: "int", nullable: true),
                    ProductItem_UrgentMinDate = table.Column<int>(type: "int", nullable: true),
                    ProductItem_UrgentMaxDate = table.Column<int>(type: "int", nullable: true),
                    ProductItem_NormalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductItem_PriceAfterSale = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductItem_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItem_Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItem_Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItem_Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductItem_Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductItem_FilePdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
