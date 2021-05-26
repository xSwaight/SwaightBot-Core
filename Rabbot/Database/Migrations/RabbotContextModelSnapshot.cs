﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Rabbot.Database;

namespace Rabbot.Database.Migrations
{
    [DbContext(typeof(RabbotContext))]
    partial class RabbotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Rabbot.Database.Rabbot.AttackEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("MessageId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("TargetId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("TargetId");

                    b.HasIndex("UserId");

                    b.ToTable("Attacks");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.AvatarLogEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AvatarId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.ToTable("AvatarLogs");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.BadWordEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BadWord")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("BadWords");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.CombiEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Accepted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<ulong>("CombiUserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("MessageId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("CombiUserId");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Combis");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.CurrentDayEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("CurrentDay");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.EasterEventEntity", b =>
                {
                    b.Property<ulong>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime?>("CatchTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DespawnTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("SpawnTime")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong?>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("MessageId");

                    b.HasIndex("UserId");

                    b.ToTable("EasterEvents");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.EventEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BonusPercent")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.FeatureEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Attacks")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("CombiExp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Eggs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Exp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("GainExp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<int>("Goats")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<bool>("HasLeft")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("LastDaily")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastMessage")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Locked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("Loses")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Profit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Spins")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("StreakLevel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("TodaysWords")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("TotalWords")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Trades")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("Wins")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.GoodWordEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("GoodWord")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("GoodWords");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.GuildEntity", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("BotChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("GuildName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Level")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<ulong?>("LevelChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<bool>("Log")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<ulong?>("LogChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong?>("NotificationChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<bool>("Notify")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<ulong?>("StreamChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<bool>("Trash")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<ulong?>("TrashChannelId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("GuildId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.InventoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Durability")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FeatureId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("ItemId");

                    b.ToTable("Inventorys");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.ItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Atk")
                        .HasColumnType("int");

                    b.Property<int>("Def")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.MusicrankEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("Seconds")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned")
                        .HasDefaultValue(0ul);

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Musicranks");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.MutedUserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Duration")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Roles")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("MutedUsers");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.NamechangeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NewName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .IsUnicode(true);

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Namechanges");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.OfficialPlayerEntity", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Playercount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("OfficialPlayers");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.PotEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Goats")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Pots");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RandomAnswerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Answer")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("RandomAnswers");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RemnantsPlayerEntity", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Playercount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Remnantsplayers");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RulesAcceptEntity", b =>
                {
                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("AcceptWord")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("bigint unsigned");

                    b.Property<ulong>("RoleId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("GuildId");

                    b.ToTable("RulesAcceptSettings");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.SongEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.StreamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<ulong?>("AnnouncedGuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("StreamId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("TwitchUserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("AnnouncedGuildId");

                    b.ToTable("Streams");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.TwitchChannelEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ChannelName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("TwitchChannels");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.UserEntity", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .IsUnicode(true);

                    b.Property<bool>("Notify")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.WarningEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Counter")
                        .HasColumnType("int");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<DateTime>("Until")
                        .HasColumnType("datetime(6)");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Warnings");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.YouTubeVideoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("VideoId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("VideoTitle")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("YouTubeVideos");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.AttackEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Attacks")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "Target")
                        .WithMany("AttackTargets")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("AttackUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.BadWordEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("BadWords")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.CombiEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "CombiUser")
                        .WithMany("CombiCombiUsers")
                        .HasForeignKey("CombiUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Combis")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("CombiUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.EasterEventEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("EasterEvents")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.FeatureEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Features")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("Features")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.GoodWordEntry", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("GoodWords")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.InventoryEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.FeatureEntity", "Feature")
                        .WithMany("Inventory")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.ItemEntity", "Item")
                        .WithMany("Inventory")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.MusicrankEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Musicranks")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("Musicranks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.MutedUserEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("MutedUsers")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("MutedUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.NamechangeEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("Namechanges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.PotEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Pots")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("Pots")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RoleEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Roles")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.RulesAcceptEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Rules")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.StreamEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Streams")
                        .HasForeignKey("AnnouncedGuildId");
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.TwitchChannelEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("TwitchChannels")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Rabbot.Database.Rabbot.WarningEntity", b =>
                {
                    b.HasOne("Rabbot.Database.Rabbot.GuildEntity", "Guild")
                        .WithMany("Warnings")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Rabbot.Database.Rabbot.UserEntity", "User")
                        .WithMany("Warnings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
