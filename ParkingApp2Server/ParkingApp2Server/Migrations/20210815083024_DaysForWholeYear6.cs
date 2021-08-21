using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkingApp2Server.Migrations
{
    public partial class DaysForWholeYear6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayTenant");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    DayId = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.DayId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });           

            migrationBuilder.CreateTable(
                name: "DayTenant",
                columns: table => new
                {
                    DaysDayId = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    TenantsTenantId = table.Column<string>(type: "nvarchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayTenant", x => new { x.DaysDayId, x.TenantsTenantId });
                    table.ForeignKey(
                        name: "FK_DayTenant_Days_DaysDayId",
                        column: x => x.DaysDayId,
                        principalTable: "Days",
                        principalColumn: "DayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DayTenant_Tenants_TenantsTenantId",
                        column: x => x.TenantsTenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Cascade);
                });
           
            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "3012",
                    "0609",
                    "0509",
                    "0409",
                    "0309",
                    "0209",
                    "0109",
                    "3108",
                    "3008",
                    "0709",
                    "2908",
                    "2708",
                    "2608",
                    "2508",
                    "2408",
                    "2308",
                    "2208",
                    "2108",
                    "2008",
                    "2808",
                    "0809",
                    "0909",
                    "1009",
                    "2909",
                    "2809",
                    "2709",
                    "2609",
                    "2509",
                    "2409",
                    "2309",
                    "2209",
                    "2109",
                    "2009",
                    "1909",
                    "1809",
                    "1709",
                    "1609",
                    "1509",
                    "1409",
                    "1309"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "1209",
                    "1109",
                    "1908",
                    "1808",
                    "1708",
                    "1608",
                    "2307",
                    "2207",
                    "2107",
                    "2007",
                    "1907",
                    "1807",
                    "1707",
                    "1607",
                    "1507",
                    "1407",
                    "1307",
                    "1207",
                    "1107",
                    "1007",
                    "0907",
                    "0807",
                    "0707",
                    "0607",
                    "0507",
                    "2407",
                    "3009",
                    "2507",
                    "2707",
                    "1508",
                    "1408",
                    "1308",
                    "1208",
                    "1108",
                    "1008",
                    "0908",
                    "0808",
                    "0708",
                    "0608",
                    "0508",
                    "0408",
                    "0308"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "0208",
                    "0108",
                    "3107",
                    "3007",
                    "2907",
                    "2807",
                    "2607",
                    "3112",
                    "0110",
                    "0310",
                    "0612",
                    "0512",
                    "0412",
                    "0312",
                    "0212",
                    "0112",
                    "3011",
                    "2911",
                    "0712",
                    "2811",
                    "2611",
                    "2511",
                    "2411",
                    "2311",
                    "2211",
                    "2111",
                    "2011",
                    "1911",
                    "2711",
                    "0812",
                    "0912",
                    "1012",
                    "2912",
                    "2812",
                    "2712",
                    "2612",
                    "2512",
                    "2412",
                    "2312",
                    "2212",
                    "2112",
                    "2012"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "1912",
                    "1812",
                    "1712",
                    "1612",
                    "1512",
                    "1412",
                    "1312",
                    "1212",
                    "1112",
                    "1811",
                    "1711",
                    "1611",
                    "1511",
                    "2210",
                    "2110",
                    "2010",
                    "1910",
                    "1810",
                    "1710",
                    "1610",
                    "1510",
                    "1410",
                    "1310",
                    "1210",
                    "1110",
                    "1010",
                    "0910",
                    "0810",
                    "0710",
                    "0610",
                    "0510",
                    "0410",
                    "2310",
                    "0407",
                    "2410",
                    "2610",
                    "1411",
                    "1311",
                    "1211",
                    "1111",
                    "1011",
                    "0911"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "0811",
                    "0711",
                    "0611",
                    "0511",
                    "0411",
                    "0311",
                    "0211",
                    "0111",
                    "3110",
                    "3010",
                    "2910",
                    "2810",
                    "2710",
                    "2510",
                    "0210",
                    "0307",
                    "0107",
                    "0603",
                    "0503",
                    "0403",
                    "0303",
                    "0203",
                    "0103",
                    "2902",
                    "2802",
                    "2702",
                    "2602",
                    "2502",
                    "2402",
                    "2302",
                    "2202",
                    "2102",
                    "2002",
                    "1902",
                    "1802",
                    "1702",
                    "0703",
                    "1602",
                    "0803",
                    "1003",
                    "2903",
                    "2803"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "2703",
                    "2603",
                    "2503",
                    "2403",
                    "2303",
                    "2203",
                    "2103",
                    "2003",
                    "1903",
                    "1803",
                    "1703",
                    "1603",
                    "1503",
                    "1403",
                    "1303",
                    "1203",
                    "1103",
                    "0903",
                    "3003",
                    "1502",
                    "1302",
                    "2001",
                    "1901",
                    "1801",
                    "1701",
                    "1601",
                    "1501",
                    "1401",
                    "1301",
                    "1201",
                    "1101",
                    "1001",
                    "0901",
                    "0801",
                    "0701",
                    "0601",
                    "0501",
                    "0401",
                    "0301",
                    "0201",
                    "2101",
                    "1402"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "2201",
                    "2401",
                    "1202",
                    "1102",
                    "1002",
                    "0902",
                    "0802",
                    "0702",
                    "0602",
                    "0502",
                    "0402",
                    "0302",
                    "0202",
                    "0102",
                    "3101",
                    "3001",
                    "2901",
                    "2801",
                    "2701",
                    "2601",
                    "2501",
                    "2301",
                    "0207",
                    "3103",
                    "0204",
                    "0706",
                    "0606",
                    "0506",
                    "0406",
                    "0306",
                    "0206",
                    "0106",
                    "3105",
                    "3005",
                    "2905",
                    "2805",
                    "2705",
                    "2605",
                    "2505",
                    "2405",
                    "2305",
                    "2205"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "2105",
                    "2005",
                    "0806",
                    "1905",
                    "0906",
                    "1106",
                    "3006",
                    "2906",
                    "2806",
                    "2706",
                    "2606",
                    "2506",
                    "2406",
                    "2306",
                    "2206",
                    "2106",
                    "2006",
                    "1906",
                    "1806",
                    "1706",
                    "1606",
                    "1506",
                    "1406",
                    "1306",
                    "1206",
                    "1006",
                    "0104",
                    "1805",
                    "1605",
                    "2104",
                    "2004",
                    "1904",
                    "1804",
                    "1704",
                    "1604",
                    "1504",
                    "1404",
                    "1304",
                    "1204",
                    "1104",
                    "1004",
                    "0904"
                });

            migrationBuilder.InsertData(
                table: "Days",
                column: "DayId",
                values: new object[]
                {
                    "0804",
                    "0704",
                    "0604",
                    "0504",
                    "0404",
                    "0304",
                    "2204",
                    "1705",
                    "2304",
                    "2504",
                    "1505",
                    "1405",
                    "1305",
                    "1205",
                    "1105",
                    "1005",
                    "0905",
                    "0805",
                    "2404",
                    "0605",
                    "0705",
                    "0405",
                    "0305",
                    "0205",
                    "0105",
                    "3004",
                    "2904",
                    "2804",
                    "2704",
                    "2604",
                    "0505",
                    "0101"
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                column: "TenantId",
                values: new object[]
                {
                    "Admin",
                    "Test"
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true,
            //    filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true,
            //    filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DayTenant_TenantsTenantId",
                table: "DayTenant",
                column: "TenantsTenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DayTenant");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
