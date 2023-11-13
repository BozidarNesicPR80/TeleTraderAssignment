﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TeleTraderAssignment.Database;

#nullable disable

namespace TeleTraderAssignment.Migrations
{
    [DbContext(typeof(NewDbContext))]
    partial class NewDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("TeleTraderAssignment.Models.Exchange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Exchange");
                });

            modelBuilder.Entity("TeleTraderAssignment.Models.Symbol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("TEXT");

                    b.Property<int>("ExchangeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Isin")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Price")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("PriceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeId");

                    b.HasIndex("TypeId");

                    b.ToTable("Symbol");
                });

            modelBuilder.Entity("TeleTraderAssignment.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Type");
                });

            modelBuilder.Entity("TeleTraderAssignment.Models.Symbol", b =>
                {
                    b.HasOne("TeleTraderAssignment.Models.Exchange", "Exchange")
                        .WithMany()
                        .HasForeignKey("ExchangeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TeleTraderAssignment.Models.Type", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exchange");

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}
