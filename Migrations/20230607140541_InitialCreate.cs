using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BrewTrack.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "api_sources",
                columns: table => new
                {
                    ApiSourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApiSourceName = table.Column<string>(type: "longtext", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_sources", x => x.ApiSourceId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "brewpubs",
                columns: table => new
                {
                    brewPubId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Longitude = table.Column<string>(type: "longtext", nullable: true),
                    Latitude = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Brewery_Type = table.Column<string>(type: "longtext", nullable: true),
                    Website_Url = table.Column<string>(type: "longtext", nullable: true),
                    Phone = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brewpubs", x => x.brewPubId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_history",
                columns: table => new
                {
                    UserHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    LastPage = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_history", x => x.UserHistoryId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(255)", nullable: false),
                    GivenName = table.Column<string>(type: "longtext", nullable: false),
                    FamilyName = table.Column<string>(type: "longtext", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "weather_forcast_header",
                columns: table => new
                {
                    WeatherForecastHeaderId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Longitude = table.Column<string>(type: "varchar(255)", nullable: false),
                    Latitude = table.Column<string>(type: "varchar(255)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CachedTimeLineId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weather_forcast_header", x => x.WeatherForecastHeaderId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cached_timeline",
                columns: table => new
                {
                    CachedTimeLineId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApiSourceRefId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cached_timeline", x => x.CachedTimeLineId);
                    table.ForeignKey(
                        name: "FK_cached_timeline_api_sources_ApiSourceRefId",
                        column: x => x.ApiSourceRefId,
                        principalTable: "api_sources",
                        principalColumn: "ApiSourceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                name: "IX_cached_timeline_ApiSourceRefId",
                table: "cached_timeline",
                column: "ApiSourceRefId");

            migrationBuilder.CreateIndex(
                name: "IX_cached_timeline_Date",
                table: "cached_timeline",
                column: "Date",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_users_EmailAddress",
                table: "users",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weather_forcast_header_Latitude_Longitude",
                table: "weather_forcast_header",
                columns: new[] { "Latitude", "Longitude" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_weather_forecast_details_WeatherForeCastHeaderId",
                table: "weather_forecast_details",
                column: "WeatherForeCastHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "brewpubs");

            migrationBuilder.DropTable(
                name: "cached_timeline");

            migrationBuilder.DropTable(
                name: "user_history");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "weather_forecast_details");

            migrationBuilder.DropTable(
                name: "api_sources");

            migrationBuilder.DropTable(
                name: "weather_forcast_header");
        }
    }
}
