using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class cityTown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Town_CityId",
                table: "Town",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Town_City_CityId",
                table: "Town",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Town_City_CityId",
                table: "Town");

            migrationBuilder.DropIndex(
                name: "IX_Town_CityId",
                table: "Town");
        }
    }
}
