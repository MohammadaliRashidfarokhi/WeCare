using Microsoft.EntityFrameworkCore.Migrations;

namespace PCR.Migrations.ApplicationDBContext2Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    AuditId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControllerName = table.Column<string>(nullable: true),
                    ActionName = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    VisitedAt = table.Column<string>(nullable: true),
                    LoggedInAt = table.Column<string>(nullable: true),
                    LoggedOutAt = table.Column<string>(nullable: true),
                    LoginStatus = table.Column<string>(nullable: true),
                    PageAccessed = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true),
                    UrlReferrer = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.AuditId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");
        }
    }
}
