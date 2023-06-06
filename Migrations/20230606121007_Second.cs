using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrewTrack.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("961356b3-e822-4824-8c2d-95467492de4d"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("b897ceee-c082-4522-bbb7-18dc95105bea"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "user_history",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("3e95d8d8-3717-49a8-8918-6b272d409060"), "Weather", new DateTime(2023, 6, 6, 12, 10, 7, 677, DateTimeKind.Utc).AddTicks(3244) },
                    { new Guid("c66efa9d-b675-4eb8-b631-7061e62d3df5"), "Breweries", new DateTime(2023, 6, 6, 12, 10, 7, 677, DateTimeKind.Utc).AddTicks(3248) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("3e95d8d8-3717-49a8-8918-6b272d409060"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("c66efa9d-b675-4eb8-b631-7061e62d3df5"));

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "user_history");

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("961356b3-e822-4824-8c2d-95467492de4d"), "Breweries", new DateTime(2023, 6, 4, 16, 54, 27, 866, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("b897ceee-c082-4522-bbb7-18dc95105bea"), "Weather", new DateTime(2023, 6, 4, 16, 54, 27, 866, DateTimeKind.Utc).AddTicks(3700) }
                });
        }
    }
}
