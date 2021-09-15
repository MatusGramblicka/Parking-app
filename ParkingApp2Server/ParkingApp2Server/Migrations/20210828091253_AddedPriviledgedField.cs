using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingApp2Server.Migrations
{
    public partial class AddedPriviledgedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "966246e1-c651-4d1e-9905-20ddb37d1608");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e8dd91e-ebad-452c-8ed1-820febe7a9f6");

            migrationBuilder.AddColumn<bool>(
                name: "Priviledged",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "6a36d105-ab8a-4e32-8aea-f803414c500e", "838b61e7-e663-4dcd-9d3d-2ecd8b4c069f", "Viewer", "VIEWER" });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "99c09a8c-6a8e-4e0a-bb22-9b5d925913f2", "f7a730f9-adff-4784-8f61-c3eab63ef537", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a36d105-ab8a-4e32-8aea-f803414c500e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99c09a8c-6a8e-4e0a-bb22-9b5d925913f2");

            migrationBuilder.DropColumn(
                name: "Priviledged",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "966246e1-c651-4d1e-9905-20ddb37d1608", "146c71db-eb27-43da-94a3-e9abd4198a38", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9e8dd91e-ebad-452c-8ed1-820febe7a9f6", "98aaeeb3-8f22-4033-a0b5-8bf39047d44d", "Administrator", "ADMINISTRATOR" });
        }
    }
}
