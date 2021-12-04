using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingApp2Server.Migrations
{
    public partial class webHookSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a36d105-ab8a-4e32-8aea-f803414c500e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99c09a8c-6a8e-4e0a-bb22-9b5d925913f2");

            migrationBuilder.CreateTable(
                name: "WebHookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebHookUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SigningSecret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureHeaderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSendAttemptCount = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    FailureHandlingStrategyFlags = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebHookSubscriptions", x => x.Id);
                });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "2dcfa671-8065-4754-aef3-1ce3645172ca", "96de0e9e-7a3a-437e-8e84-ec9c3af0e2d4", "Administrator", "ADMINISTRATOR" });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "7846aefe-15e3-48d9-96f0-9554c1b7d355", "610c3ec4-956b-4f9f-9215-d973b4286974", "Viewer", "VIEWER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebHookSubscriptions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2dcfa671-8065-4754-aef3-1ce3645172ca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7846aefe-15e3-48d9-96f0-9554c1b7d355");

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "6a36d105-ab8a-4e32-8aea-f803414c500e", "838b61e7-e663-4dcd-9d3d-2ecd8b4c069f", "Viewer", "VIEWER" });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[] { "99c09a8c-6a8e-4e0a-bb22-9b5d925913f2", "f7a730f9-adff-4784-8f61-c3eab63ef537", "Administrator", "ADMINISTRATOR" });
        }
    }
}
