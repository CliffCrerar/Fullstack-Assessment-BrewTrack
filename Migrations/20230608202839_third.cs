using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrewTrack.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_forcast_day_weather_forcast_header_WeatherForcastMet~",
                table: "weather_forcast_day");

            migrationBuilder.DropForeignKey(
                name: "FK_weather_forcast_header_cached_timeline_CachedTimeLineId",
                table: "weather_forcast_header");

            migrationBuilder.DropIndex(
                name: "IX_weather_forcast_header_CachedTimeLineId",
                table: "weather_forcast_header");

            migrationBuilder.DropIndex(
                name: "IX_weather_forcast_day_WeatherForcastMetaId",
                table: "weather_forcast_day");

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("954d520c-d406-49ac-91e1-ba224ec59d12"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("b367a7ec-a264-4aaa-b523-885bd9f5ded2"));

            migrationBuilder.DropColumn(
                name: "WeatherForcastMetaId",
                table: "weather_forcast_day");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "weather_forcast_header",
                newName: "Lng");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "weather_forcast_header",
                newName: "Lat");

            migrationBuilder.RenameIndex(
                name: "IX_weather_forcast_header_Latitude_Longitude",
                table: "weather_forcast_header",
                newName: "IX_weather_forcast_header_Lat_Lng");

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "weather_forcast_header",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DailyQuota",
                table: "weather_forcast_header",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "End",
                table: "weather_forcast_header",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "RequestCount",
                table: "weather_forcast_header",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Start",
                table: "weather_forcast_header",
                type: "longtext",
                nullable: false);

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("250f07e5-4cbc-4163-b857-b0acb0082e51"), "Weather", new DateTime(2023, 6, 8, 20, 28, 38, 926, DateTimeKind.Utc).AddTicks(9050) },
                    { new Guid("8bebdfed-3d40-473b-bcab-5f771ea4f3ac"), "Breweries", new DateTime(2023, 6, 8, 20, 28, 38, 926, DateTimeKind.Utc).AddTicks(9050) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("250f07e5-4cbc-4163-b857-b0acb0082e51"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("8bebdfed-3d40-473b-bcab-5f771ea4f3ac"));

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "weather_forcast_header");

            migrationBuilder.DropColumn(
                name: "DailyQuota",
                table: "weather_forcast_header");

            migrationBuilder.DropColumn(
                name: "End",
                table: "weather_forcast_header");

            migrationBuilder.DropColumn(
                name: "RequestCount",
                table: "weather_forcast_header");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "weather_forcast_header");

            migrationBuilder.RenameColumn(
                name: "Lng",
                table: "weather_forcast_header",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Lat",
                table: "weather_forcast_header",
                newName: "Latitude");

            migrationBuilder.RenameIndex(
                name: "IX_weather_forcast_header_Lat_Lng",
                table: "weather_forcast_header",
                newName: "IX_weather_forcast_header_Latitude_Longitude");

            migrationBuilder.AddColumn<Guid>(
                name: "WeatherForcastMetaId",
                table: "weather_forcast_day",
                type: "char(36)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("954d520c-d406-49ac-91e1-ba224ec59d12"), "Weather", new DateTime(2023, 6, 8, 19, 22, 6, 234, DateTimeKind.Utc).AddTicks(3410) },
                    { new Guid("b367a7ec-a264-4aaa-b523-885bd9f5ded2"), "Breweries", new DateTime(2023, 6, 8, 19, 22, 6, 234, DateTimeKind.Utc).AddTicks(3420) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_weather_forcast_header_CachedTimeLineId",
                table: "weather_forcast_header",
                column: "CachedTimeLineId");

            migrationBuilder.CreateIndex(
                name: "IX_weather_forcast_day_WeatherForcastMetaId",
                table: "weather_forcast_day",
                column: "WeatherForcastMetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_forcast_day_weather_forcast_header_WeatherForcastMet~",
                table: "weather_forcast_day",
                column: "WeatherForcastMetaId",
                principalTable: "weather_forcast_header",
                principalColumn: "WeatherForecastHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_forcast_header_cached_timeline_CachedTimeLineId",
                table: "weather_forcast_header",
                column: "CachedTimeLineId",
                principalTable: "cached_timeline",
                principalColumn: "CachedTimeLineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
