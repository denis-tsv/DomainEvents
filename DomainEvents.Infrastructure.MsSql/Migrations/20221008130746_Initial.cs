using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainEvents.Infrastructure.MsSql.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountGroupAccounts",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    AccountGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroupAccounts", x => new { x.AccountId, x.AccountGroupId });
                    table.ForeignKey(
                        name: "FK_AccountGroupAccounts_AccountGroups_AccountGroupId",
                        column: x => x.AccountGroupId,
                        principalTable: "AccountGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountGroupAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGroupAccounts_AccountGroupId",
                table: "AccountGroupAccounts",
                column: "AccountGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGroupAccounts");

            migrationBuilder.DropTable(
                name: "AccountGroups");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
