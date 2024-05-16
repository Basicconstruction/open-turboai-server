﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Turbo_Auth.Context;

#nullable disable

namespace Turbo_Auth.Entities
{
    [DbContext(typeof(AuthContext))]
    partial class AuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Turbo_Auth.Models.Accounts.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.Accounts.AccountRole", b =>
                {
                    b.Property<int>("AccountRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("AccountRoleId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRoles", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.Accounts.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatHistory", b =>
                {
                    b.Property<long>("ChatHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("DataId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChatHistoryId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatHistories", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatMessage", b =>
                {
                    b.Property<long>("ChatMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("ChatHistoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<long>("DataId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Finish")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Model")
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<int>("ShowType")
                        .HasColumnType("int");

                    b.HasKey("ChatMessageId");

                    b.HasIndex("ChatHistoryId");

                    b.ToTable("ChatMessages", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.FileAdds", b =>
                {
                    b.Property<long>("FileAddsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<long>("ChatMessageId")
                        .HasColumnType("bigint");

                    b.Property<string>("FileContent")
                        .HasColumnType("longtext");

                    b.Property<string>("FileName")
                        .HasColumnType("longtext");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileType")
                        .HasColumnType("longtext");

                    b.Property<string>("ParsedContent")
                        .HasColumnType("longtext");

                    b.HasKey("FileAddsId");

                    b.HasIndex("ChatMessageId");

                    b.ToTable("FileAdds", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.Tasks.GenerateTask", b =>
                {
                    b.Property<string>("TaskId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("TaskType")
                        .HasColumnType("longtext");

                    b.HasKey("TaskId");

                    b.HasIndex("AccountId");

                    b.ToTable("GenerateTasks", (string)null);
                });

            modelBuilder.Entity("Turbo_Auth.Models.Accounts.AccountRole", b =>
                {
                    b.HasOne("Turbo_Auth.Models.Accounts.Account", "Account")
                        .WithMany("UserRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Turbo_Auth.Models.Accounts.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatHistory", b =>
                {
                    b.HasOne("Turbo_Auth.Models.Accounts.Account", "Account")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatMessage", b =>
                {
                    b.HasOne("Turbo_Auth.Models.ClientSyncs.Messages.ChatHistory", "ChatHistory")
                        .WithMany("ChatMessages")
                        .HasForeignKey("ChatHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatHistory");
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.FileAdds", b =>
                {
                    b.HasOne("Turbo_Auth.Models.ClientSyncs.Messages.ChatMessage", "ChatMessage")
                        .WithMany("FileList")
                        .HasForeignKey("ChatMessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatMessage");
                });

            modelBuilder.Entity("Turbo_Auth.Models.Tasks.GenerateTask", b =>
                {
                    b.HasOne("Turbo_Auth.Models.Accounts.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Turbo_Auth.Models.Accounts.Account", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatHistory", b =>
                {
                    b.Navigation("ChatMessages");
                });

            modelBuilder.Entity("Turbo_Auth.Models.ClientSyncs.Messages.ChatMessage", b =>
                {
                    b.Navigation("FileList");
                });
#pragma warning restore 612, 618
        }
    }
}
