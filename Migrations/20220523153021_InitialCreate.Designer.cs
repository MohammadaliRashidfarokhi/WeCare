﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PCR.Models;

namespace PCR.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20220523153021_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PCR.Models.Clinic", b =>
                {
                    b.Property<string>("ClinicID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClinicName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClinicID");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("PCR.Models.Report", b =>
                {
                    b.Property<string>("Reportid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<string>("Citizenship")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Clinique")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfObservation")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailConf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassportNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonalIdentityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phonenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TestType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("checkedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Reportid");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("PCR.Models.Sample", b =>
                {
                    b.Property<int>("SampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Reportid")
                        .HasColumnType("int");

                    b.Property<string>("Reportid1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SampleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("checkedBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SampleId");

                    b.HasIndex("Reportid1");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("PCR.Models.User", b =>
                {
                    b.Property<string>("Userid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Clicicid")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Src")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Userid");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PCR.Models.Sample", b =>
                {
                    b.HasOne("PCR.Models.Report", null)
                        .WithMany("Samples")
                        .HasForeignKey("Reportid1");
                });
#pragma warning restore 612, 618
        }
    }
}
