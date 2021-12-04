using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingApp2Server.Migrations
{
    public partial class webhooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2dcfa671-8065-4754-aef3-1ce3645172ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7846aefe-15e3-48d9-96f0-9554c1b7d355");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "482f6603-e659-4950-80cb-16d3e7acb26d", "d1e243d9-0eea-447d-91f4-5340c47d90da", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "be1e84cd-df83-4498-8347-3571aceea6a7", "568bc35f-d4c7-40af-917a-c80df6dc3c6b", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "482f6603-e659-4950-80cb-16d3e7acb26d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "be1e84cd-df83-4498-8347-3571aceea6a7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2dcfa671-8065-4754-aef3-1ce3645172ca", "96de0e9e-7a3a-437e-8e84-ec9c3af0e2d4", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7846aefe-15e3-48d9-96f0-9554c1b7d355", "610c3ec4-956b-4f9f-9215-d973b4286974", "Viewer", "VIEWER" });
        }
    }
}
