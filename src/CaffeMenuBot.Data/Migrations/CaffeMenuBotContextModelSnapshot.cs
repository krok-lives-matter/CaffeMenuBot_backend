﻿// <auto-generated />
using CaffeMenuBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CaffeMenuBot.Data.Migrations
{
    [DbContext(typeof(CaffeMenuBotContext))]
    partial class CaffeMenuBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Authentication.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_role");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_salt");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("app_users", "public");
                });

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Bot.BotUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("user_phone");

                    b.HasKey("Id");

                    b.ToTable("bot_users", "public");
                });

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Menu.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("category_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category_name");

                    b.HasKey("Id");

                    b.ToTable("categories", "public");
                });

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Menu.Dish", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("dish_id")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("DishName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("dish_name");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(5, 2)")
                        .HasColumnName("price");

                    b.Property<string>("Serving")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("serving");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("dishes", "public");
                });

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Menu.Dish", b =>
                {
                    b.HasOne("CaffeMenuBot.Data.Models.Menu.Category", "Category")
                        .WithMany("Dishes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("CaffeMenuBot.Data.Models.Menu.Category", b =>
                {
                    b.Navigation("Dishes");
                });
#pragma warning restore 612, 618
        }
    }
}
