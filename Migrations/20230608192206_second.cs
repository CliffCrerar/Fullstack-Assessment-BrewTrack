using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrewTrack.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weather_forecast_details");

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("340e1ee2-3171-4f09-b06f-408e6fa149ca"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("cf63956f-f675-4121-a6f8-26dfb40bd750"));

            migrationBuilder.CreateTable(
                name: "weather_forcast_day",
                columns: table => new
                {
                    WeatherForecastDayId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    FullDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AverageTemperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeatherForcastMetaRefId = table.Column<Guid>(type: "char(36)", nullable: false),
                    WeatherForcastMetaId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_forcast_day", x => x.WeatherForecastDayId);
                    table.ForeignKey(
                        name: "FK_weather_forcast_day_weather_forcast_header_WeatherForcastMet~",
                        column: x => x.WeatherForcastMetaId,
                        principalTable: "weather_forcast_header",
                        principalColumn: "WeatherForecastHeaderId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "weather_forcast_hour",
                columns: table => new
                {
                    WeatherForcastHourId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    FullDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AirTemperature = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WeatherForeCastDayRefId = table.Column<Guid>(type: "char(36)", nullable: false),
                    WeatherForecastDayId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_forcast_hour", x => x.WeatherForcastHourId);
                    table.ForeignKey(
                        name: "FK_weather_forcast_hour_weather_forcast_day_WeatherForecastDayId",
                        column: x => x.WeatherForecastDayId,
                        principalTable: "weather_forcast_day",
                        principalColumn: "WeatherForecastDayId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_weather_forcast_hour_WeatherForecastDayId",
                table: "weather_forcast_hour",
                column: "WeatherForecastDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_weather_forcast_header_cached_timeline_CachedTimeLineId",
                table: "weather_forcast_header",
                column: "CachedTimeLineId",
                principalTable: "cached_timeline",
                principalColumn: "CachedTimeLineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_weather_forcast_header_cached_timeline_CachedTimeLineId",
                table: "weather_forcast_header");

            migrationBuilder.DropTable(
                name: "weather_forcast_hour");

            migrationBuilder.DropTable(
                name: "weather_forcast_day");

            migrationBuilder.DropIndex(
                name: "IX_weather_forcast_header_CachedTimeLineId",
                table: "weather_forcast_header");

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("954d520c-d406-49ac-91e1-ba224ec59d12"));

            migrationBuilder.DeleteData(
                table: "api_sources",
                keyColumn: "ApiSourceId",
                keyValue: new Guid("b367a7ec-a264-4aaa-b523-885bd9f5ded2"));

            migrationBuilder.CreateTable(
                name: "weather_forecast_details",
                columns: table => new
                {
                    WeatherForeCastDetailsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AirTemprature = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WeatherForCastHeaderId = table.Column<Guid>(type: "char(36)", nullable: false),
                    WeatherForeCastHeaderId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_forecast_details", x => x.WeatherForeCastDetailsId);
                    table.ForeignKey(
                        name: "FK_weather_forecast_details_weather_forcast_header_WeatherForeC~",
                        column: x => x.WeatherForeCastHeaderId,
                        principalTable: "weather_forcast_header",
                        principalColumn: "WeatherForecastHeaderId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("340e1ee2-3171-4f09-b06f-408e6fa149ca"), "Breweries", new DateTime(2023, 6, 7, 14, 5, 41, 60, DateTimeKind.Utc).AddTicks(8127) },
                    { new Guid("cf63956f-f675-4121-a6f8-26dfb40bd750"), "Weather", new DateTime(2023, 6, 7, 14, 5, 41, 60, DateTimeKind.Utc).AddTicks(8118) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_weather_forecast_details_WeatherForeCastHeaderId",
                table: "weather_forecast_details",
                column: "WeatherForeCastHeaderId");
        }
    }
}
