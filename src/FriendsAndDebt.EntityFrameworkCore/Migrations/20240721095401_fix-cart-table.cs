using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FriendsAndDebt.Migrations
{
    /// <inheritdoc />
    public partial class fixcarttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cards_OwnerId",
                table: "Cards",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Users_OwnerId",
                table: "Cards",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_OwnerId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_OwnerId",
                table: "Cards");
        }
    }
}
