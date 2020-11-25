using Microsoft.EntityFrameworkCore.Migrations;

namespace Ace_cars.Migrations
{
    public partial class updatetables01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_HeardAboutUs_HeardAboutUsNavigationId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "HeardAboutUs",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "HeardAboutUsNavigationId",
                table: "Customer",
                newName: "HeardAboutUsId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_HeardAboutUsNavigationId",
                table: "Customer",
                newName: "IX_Customer_HeardAboutUsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_HeardAboutUs_HeardAboutUsId",
                table: "Customer",
                column: "HeardAboutUsId",
                principalTable: "HeardAboutUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_HeardAboutUs_HeardAboutUsId",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "HeardAboutUsId",
                table: "Customer",
                newName: "HeardAboutUsNavigationId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_HeardAboutUsId",
                table: "Customer",
                newName: "IX_Customer_HeardAboutUsNavigationId");

            migrationBuilder.AddColumn<int>(
                name: "HeardAboutUs",
                table: "Customer",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_HeardAboutUs_HeardAboutUsNavigationId",
                table: "Customer",
                column: "HeardAboutUsNavigationId",
                principalTable: "HeardAboutUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
