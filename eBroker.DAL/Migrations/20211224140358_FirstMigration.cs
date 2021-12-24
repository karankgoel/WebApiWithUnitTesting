using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;

namespace eBroker.DAL.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equities",
                columns: table => new
                {
                    EquityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equities", x => x.EquityId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionOrders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EquityId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionOrders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.WalletId);
                });

            migrationBuilder.CreateTable(
                name: "EquityHoldings",
                columns: table => new
                {
                    EquityHoldingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EquityId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    WalletDBWalletId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquityHoldings", x => x.EquityHoldingId);
                    table.ForeignKey(
                        name: "FK_EquityHoldings_Wallet_WalletDBWalletId",
                        column: x => x.WalletDBWalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquityHoldings_WalletDBWalletId",
                table: "EquityHoldings",
                column: "WalletDBWalletId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equities");

            migrationBuilder.DropTable(
                name: "EquityHoldings");

            migrationBuilder.DropTable(
                name: "TransactionOrders");

            migrationBuilder.DropTable(
                name: "Wallet");
        }
    }
}
