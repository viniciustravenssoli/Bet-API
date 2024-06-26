﻿// <auto-generated />
using System;
using Bet.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bet.Infra.Migrations
{
    [DbContext(typeof(BetContext))]
    [Migration("20240321230842_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("Bet.Domain.Entities.Bet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiryTime")
                        .HasColumnType("TEXT");

                    b.Property<double>("Odd")
                        .HasColumnType("REAL");

                    b.Property<bool>("Paid")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("Bet.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Balance")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Bet.Domain.Entities.UserBet", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("BetAmount")
                        .HasColumnType("REAL");

                    b.Property<long>("BetId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BetId");

                    b.HasIndex("UserId");

                    b.ToTable("UserBets");
                });

            modelBuilder.Entity("Bet.Domain.Entities.UserBet", b =>
                {
                    b.HasOne("Bet.Domain.Entities.Bet", "Bet")
                        .WithMany("UserBets")
                        .HasForeignKey("BetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bet.Domain.Entities.User", "User")
                        .WithMany("UserBets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bet.Domain.Entities.Bet", b =>
                {
                    b.Navigation("UserBets");
                });

            modelBuilder.Entity("Bet.Domain.Entities.User", b =>
                {
                    b.Navigation("UserBets");
                });
#pragma warning restore 612, 618
        }
    }
}
