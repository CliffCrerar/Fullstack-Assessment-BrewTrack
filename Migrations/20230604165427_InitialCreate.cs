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
                    LastPage = table.Column<int>(type: "int", nullable: false)
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
                name: "cached_timeline",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApiSourceRefId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cached_timeline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cached_timeline_api_sources_ApiSourceRefId",
                        column: x => x.ApiSourceRefId,
                        principalTable: "api_sources",
                        principalColumn: "ApiSourceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "api_sources",
                columns: new[] { "ApiSourceId", "ApiSourceName", "DateCreated" },
                values: new object[,]
                {
                    { new Guid("961356b3-e822-4824-8c2d-95467492de4d"), "Breweries", new DateTime(2023, 6, 4, 16, 54, 27, 866, DateTimeKind.Utc).AddTicks(3703) },
                    { new Guid("b897ceee-c082-4522-bbb7-18dc95105bea"), "Weather", new DateTime(2023, 6, 4, 16, 54, 27, 866, DateTimeKind.Utc).AddTicks(3700) }
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
                name: "api_sources");
        }
    }
}
