using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoice.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalCoulmnToInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Invoices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "InvoiceItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "InvoiceItems");
        }
    }
}
