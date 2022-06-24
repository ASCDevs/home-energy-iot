using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace home_energy_iot_api.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    IdDevice = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdHouseUser = table.Column<int>(type: "INTEGER", nullable: false),
                    NameDevice = table.Column<string>(type: "TEXT", nullable: true),
                    DescDevice = table.Column<string>(type: "TEXT", nullable: true),
                    DtRegistration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DtInactivation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.IdDevice);
                });

            migrationBuilder.CreateTable(
                name: "houseBills",
                columns: table => new
                {
                    IdHouseBill = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdHouseUser = table.Column<int>(type: "INTEGER", nullable: false),
                    MonthBill = table.Column<int>(type: "INTEGER", nullable: false),
                    YearBill = table.Column<int>(type: "INTEGER", nullable: false),
                    TariffBill = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValuePerKWH = table.Column<decimal>(type: "TEXT", nullable: false),
                    BaseKWH = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_houseBills", x => x.IdHouseBill);
                });

            migrationBuilder.CreateTable(
                name: "houses",
                columns: table => new
                {
                    IdHouseUser = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false),
                    HouseName = table.Column<string>(type: "TEXT", nullable: false),
                    TypeAddress = table.Column<string>(type: "TEXT", nullable: true),
                    NameAddress = table.Column<string>(type: "TEXT", nullable: true),
                    NumberAddress = table.Column<string>(type: "TEXT", nullable: true),
                    DtRegistration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PeriodDaysReport = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_houses", x => x.IdHouseUser);
                });

            migrationBuilder.CreateTable(
                name: "reportDevices",
                columns: table => new
                {
                    IdReportDevice = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDevice = table.Column<int>(type: "INTEGER", nullable: false),
                    DtReport = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValueTotal = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportDevices", x => x.IdReportDevice);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    UserCPF = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    DtRegistration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DtInactivation = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.IdUser);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "houseBills");

            migrationBuilder.DropTable(
                name: "houses");

            migrationBuilder.DropTable(
                name: "reportDevices");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
