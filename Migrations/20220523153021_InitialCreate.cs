using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PCR.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    ClinicID = table.Column<string>(nullable: false),
                    ClinicName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.ClinicID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Reportid = table.Column<string>(nullable: false),
                    TestType = table.Column<string>(nullable: true),
                    Clinique = table.Column<string>(nullable: true),
                    Patient = table.Column<string>(nullable: true),
                    DateOfObservation = table.Column<DateTime>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    PassportNumber = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConf = table.Column<string>(nullable: true),
                    Citizenship = table.Column<string>(nullable: true),
                    PersonalIdentityNumber = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: false),
                    checkedBy = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Reportid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Userid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Clicicid = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Src = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Userid);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SampleName = table.Column<string>(nullable: true),
                    Reportid = table.Column<int>(nullable: false),
                    checkedBy = table.Column<string>(nullable: true),
                    Reportid1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleId);
                    table.ForeignKey(
                        name: "FK_Samples_Reports_Reportid1",
                        column: x => x.Reportid1,
                        principalTable: "Reports",
                        principalColumn: "Reportid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Samples_Reportid1",
                table: "Samples",
                column: "Reportid1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
