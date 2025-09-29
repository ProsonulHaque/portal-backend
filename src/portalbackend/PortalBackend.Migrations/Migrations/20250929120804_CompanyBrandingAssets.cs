using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Org.Eclipse.TractusX.Portal.Backend.PortalBackend.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CompanyBrandingAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "company_branding_asset_type",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_branding_asset_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company_branding_files",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    company_branding_asset_type_id = table.Column<int>(type: "integer", nullable: false),
                    media_type_id = table.Column<int>(type: "integer", nullable: false),
                    file_size_in_bytes = table.Column<long>(type: "bigint", nullable: false),
                    file_content = table.Column<byte[]>(type: "bytea", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_last_changed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_editor_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_branding_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_branding_files_companies_company_id",
                        column: x => x.company_id,
                        principalSchema: "portal",
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_branding_files_company_branding_asset_type_company_",
                        column: x => x.company_branding_asset_type_id,
                        principalSchema: "portal",
                        principalTable: "company_branding_asset_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_branding_files_media_types_media_type_id",
                        column: x => x.media_type_id,
                        principalSchema: "portal",
                        principalTable: "media_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_branding_texts",
                schema: "portal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    branding_text = table.Column<string>(type: "text", nullable: false),
                    company_branding_asset_type_id = table.Column<int>(type: "integer", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    date_last_changed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_editor_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_branding_texts", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_branding_texts_companies_company_id",
                        column: x => x.company_id,
                        principalSchema: "portal",
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_branding_texts_company_branding_asset_type_company_",
                        column: x => x.company_branding_asset_type_id,
                        principalSchema: "portal",
                        principalTable: "company_branding_asset_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "portal",
                table: "company_branding_asset_type",
                columns: new[] { "id", "label" },
                values: new object[,]
                {
                    { 1, "LOGO" },
                    { 2, "FOOTER" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_company_branding_files_company_branding_asset_type_id",
                schema: "portal",
                table: "company_branding_files",
                column: "company_branding_asset_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_branding_files_company_id",
                schema: "portal",
                table: "company_branding_files",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_branding_files_media_type_id",
                schema: "portal",
                table: "company_branding_files",
                column: "media_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_branding_texts_company_branding_asset_type_id",
                schema: "portal",
                table: "company_branding_texts",
                column: "company_branding_asset_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_branding_texts_company_id",
                schema: "portal",
                table: "company_branding_texts",
                column: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_branding_files",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "company_branding_texts",
                schema: "portal");

            migrationBuilder.DropTable(
                name: "company_branding_asset_type",
                schema: "portal");
        }
    }
}
