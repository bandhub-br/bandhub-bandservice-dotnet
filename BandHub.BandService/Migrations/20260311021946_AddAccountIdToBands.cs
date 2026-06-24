using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BandHub.BandService.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bands",
                table: "bands");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "bands",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_bands",
                table: "bands",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_bands_AccountId",
                table: "bands",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_bands",
                table: "bands");

            migrationBuilder.DropIndex(
                name: "IX_bands_AccountId",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "bands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bands",
                table: "bands",
                column: "Id");
        }
    }
}
