﻿// <auto-generated />
using System;
using AppointmentJournal.AppDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AppointmentJournal.Migrations.AppointmentJournal
{
    [DbContext(typeof(AppointmentJournalContext))]
    partial class AppointmentJournalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Address", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("AddressValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProducerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ProducerID");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Appointment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("ConsumerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ConsumerID");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint")
                        .HasColumnName("ServiceID");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime");

                    b.Property<long>("WorkDayTimeSpanId")
                        .HasColumnType("bigint")
                        .HasColumnName("WorkDayTimeSpanID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ServiceId" }, "IX_Appointments_ServiceID");

                    b.HasIndex(new[] { "WorkDayTimeSpanId" }, "IX_Appointments_WorkDayID");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Service", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("CategoryID");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("money");

                    b.Property<string>("ProducerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ProducerID");

                    b.HasKey("Id");

                    b.HasIndex("ProducerId");

                    b.HasIndex(new[] { "CategoryId" }, "IX_Services_CategoryID");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.ServicesCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ServicesCategories");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.WorkDay", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("ProducerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("ProducerID");

                    b.HasKey("Id");

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.WorkDaysTimeSpan", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint")
                        .HasColumnName("AddressID");

                    b.Property<DateTime>("BeginTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime");

                    b.Property<long>("ServiceId")
                        .HasColumnType("bigint")
                        .HasColumnName("ServiceID");

                    b.Property<long>("WorkDayId")
                        .HasColumnType("bigint")
                        .HasColumnName("WorkDayID");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "AddressId" }, "IX_WorkDaysTimeSpans_AddressID");

                    b.HasIndex(new[] { "ServiceId" }, "IX_WorkDaysTimeSpans_ServiceID");

                    b.HasIndex(new[] { "WorkDayId" }, "IX_WorkDaysTimeSpans_WorkDayID");

                    b.ToTable("WorkDaysTimeSpans");
                });

            modelBuilder.Entity("AppointmentJournal.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Appointment", b =>
                {
                    b.HasOne("AppointmentJournal.AppDatabase.Service", "Service")
                        .WithMany("Appointments")
                        .HasForeignKey("ServiceId")
                        .IsRequired()
                        .HasConstraintName("FK_Appointments_Services");

                    b.HasOne("AppointmentJournal.AppDatabase.WorkDaysTimeSpan", "WorkDayTimeSpan")
                        .WithMany("Appointments")
                        .HasForeignKey("WorkDayTimeSpanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Appointments_WorkDaysTimeSpans");

                    b.Navigation("Service");

                    b.Navigation("WorkDayTimeSpan");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Service", b =>
                {
                    b.HasOne("AppointmentJournal.AppDatabase.ServicesCategory", "Category")
                        .WithMany("Services")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Services_ServicesCategories");

                    b.HasOne("AppointmentJournal.Models.User", "Producer")
                        .WithMany()
                        .HasForeignKey("ProducerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Producer");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.WorkDaysTimeSpan", b =>
                {
                    b.HasOne("AppointmentJournal.AppDatabase.Address", "Address")
                        .WithMany("WorkDaysTimeSpans")
                        .HasForeignKey("AddressId")
                        .HasConstraintName("FK_WorkDaysTimeSpans_Addresses");

                    b.HasOne("AppointmentJournal.AppDatabase.Service", "Service")
                        .WithMany("WorkDaysTimeSpans")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WorkDaysTimeSpans_Services");

                    b.HasOne("AppointmentJournal.AppDatabase.WorkDay", "WorkDay")
                        .WithMany("WorkDaysTimeSpans")
                        .HasForeignKey("WorkDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WorkDaysTimeSpans_WorkDays");

                    b.Navigation("Address");

                    b.Navigation("Service");

                    b.Navigation("WorkDay");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Address", b =>
                {
                    b.Navigation("WorkDaysTimeSpans");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.Service", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("WorkDaysTimeSpans");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.ServicesCategory", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.WorkDay", b =>
                {
                    b.Navigation("WorkDaysTimeSpans");
                });

            modelBuilder.Entity("AppointmentJournal.AppDatabase.WorkDaysTimeSpan", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}
