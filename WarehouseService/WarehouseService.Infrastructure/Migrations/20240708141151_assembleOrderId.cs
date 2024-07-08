using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class assembleOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AssembledVehicleStocks",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AssembledVehicleStocks");
        }
    }
}
