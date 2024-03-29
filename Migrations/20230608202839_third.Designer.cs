﻿// <auto-generated />
using System;
using BrewTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BrewTrack.Migrations
{
    [DbContext(typeof(BrewTrackDbContext))]
    [Migration("20230608202839_third")]
    partial class third
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BrewTrack.Models.ApiSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("ApiSourceId");

                    b.Property<string>("ApiSourceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("api_sources");

                    b.HasData(
                        new
                        {
                            Id = new Guid("250f07e5-4cbc-4163-b857-b0acb0082e51"),
                            ApiSourceName = "Weather",
                            DateCreated = new DateTime(2023, 6, 8, 20, 28, 38, 926, DateTimeKind.Utc).AddTicks(9050)
                        },
                        new
                        {
                            Id = new Guid("8bebdfed-3d40-473b-bcab-5f771ea4f3ac"),
                            ApiSourceName = "Breweries",
                            DateCreated = new DateTime(2023, 6, 8, 20, 28, 38, 926, DateTimeKind.Utc).AddTicks(9050)
                        });
                });

            modelBuilder.Entity("BrewTrack.Models.BrewPub", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("brewPubId");

                    b.Property<string>("Brewery_Type")
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("Latitude")
                        .HasColumnType("longtext");

                    b.Property<string>("Longitude")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Website_Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("brewpubs");
                });

            modelBuilder.Entity("BrewTrack.Models.CachedTimeline", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("CachedTimeLineId");

                    b.Property<Guid>("ApiSourceRefId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ApiSourceRefId");

                    b.HasIndex("Date")
                        .IsDescending();

                    b.ToTable("cached_timeline");
                });

            modelBuilder.Entity("BrewTrack.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("UserId");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("BrewTrack.Models.UserHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserHistoryId");

                    b.Property<int>("LastPage")
                        .HasColumnType("int");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("user_history");
                });

            modelBuilder.Entity("BrewTrack.Models.WeatherForcastMeta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("WeatherForecastHeaderId");

                    b.Property<Guid>("CachedTimeLineId")
                        .HasColumnType("char(36)");

                    b.Property<int>("Cost")
                        .HasColumnType("int");

                    b.Property<int>("DailyQuota")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("End")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Lat")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Lng")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("RequestCount")
                        .HasColumnType("int");

                    b.Property<string>("Start")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Lat", "Lng")
                        .IsUnique();

                    b.ToTable("weather_forcast_header");
                });

            modelBuilder.Entity("BrewTrack.Models.WeatherForecastDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("WeatherForecastDayId");

                    b.Property<decimal>("AverageTemperature")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<DateTime>("FullDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("WeatherForcastMetaRefId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("weather_forcast_day");
                });

            modelBuilder.Entity("BrewTrack.Models.WeatherForecastHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("WeatherForcastHourId");

                    b.Property<decimal>("AirTemperature")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("FullDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Hour")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time(6)");

                    b.Property<Guid>("WeatherForeCastDayRefId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("WeatherForecastDayId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("WeatherForecastDayId");

                    b.ToTable("weather_forcast_hour");
                });

            modelBuilder.Entity("BrewTrack.Models.CachedTimeline", b =>
                {
                    b.HasOne("BrewTrack.Models.ApiSource", null)
                        .WithMany("Timeline")
                        .HasForeignKey("ApiSourceRefId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BrewTrack.Models.WeatherForecastHour", b =>
                {
                    b.HasOne("BrewTrack.Models.WeatherForecastDay", null)
                        .WithMany("Temperatures")
                        .HasForeignKey("WeatherForecastDayId");
                });

            modelBuilder.Entity("BrewTrack.Models.ApiSource", b =>
                {
                    b.Navigation("Timeline");
                });

            modelBuilder.Entity("BrewTrack.Models.WeatherForecastDay", b =>
                {
                    b.Navigation("Temperatures");
                });
#pragma warning restore 612, 618
        }
    }
}
