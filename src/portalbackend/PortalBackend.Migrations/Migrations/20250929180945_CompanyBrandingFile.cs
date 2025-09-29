using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CompanyBrandingFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file_size_in_bytes",
                schema: "portal",
                table: "company_branding_files",
                newName: "file_size_in_kilo_byte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file_size_in_kilo_byte",
                schema: "portal",
                table: "company_branding_files",
                newName: "file_size_in_bytes");
        }
    }
}
