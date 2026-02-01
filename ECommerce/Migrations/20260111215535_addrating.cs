using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce_API.Migrations
{
    /// <inheritdoc />
    public partial class addrating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "OrderDetails",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "OrderDetails");
        }
    }
}
