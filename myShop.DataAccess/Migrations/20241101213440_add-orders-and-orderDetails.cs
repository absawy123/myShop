using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addordersandorderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Orders",
                newName: "UserPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Orders",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "UserAddress");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserPhoneNumber",
                table: "Orders",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Orders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "UserAddress",
                table: "Orders",
                newName: "Address");
        }
    }
}
