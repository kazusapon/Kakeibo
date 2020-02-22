using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Eri.Migrations
{
    public partial class Eri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mst_Available",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Money = table.Column<int>(nullable: false),
                    Del_Flag = table.Column<bool>(nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Available", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Fixed",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fixed_Name = table.Column<string>(nullable: true),
                    Money = table.Column<int>(nullable: false),
                    Del_Flag = table.Column<bool>(nullable: false),
                    Start_Date = table.Column<DateTime>(nullable: false),
                    End_Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Fixed", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Income",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Income_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Income", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_Spend",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Spend_Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_Spend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mst_User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User_Name = table.Column<string>(nullable: true),
                    Login_Id = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mst_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tra_Income",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User_Id = table.Column<int>(nullable: false),
                    Income_Id = table.Column<int>(nullable: false),
                    Money = table.Column<int>(nullable: false),
                    Payment_Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Linked_Flag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tra_Income", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tra_Spending",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    User_Id = table.Column<int>(nullable: false),
                    Spend_Id = table.Column<int>(nullable: false),
                    Money = table.Column<int>(nullable: false),
                    Purchase_Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Linked_Flag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tra_Spending", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mst_Available");

            migrationBuilder.DropTable(
                name: "Mst_Fixed");

            migrationBuilder.DropTable(
                name: "Mst_Income");

            migrationBuilder.DropTable(
                name: "Mst_Spend");

            migrationBuilder.DropTable(
                name: "Mst_User");

            migrationBuilder.DropTable(
                name: "Tra_Income");

            migrationBuilder.DropTable(
                name: "Tra_Spending");
        }
    }
}
