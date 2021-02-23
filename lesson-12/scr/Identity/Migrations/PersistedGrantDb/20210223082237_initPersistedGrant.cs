using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Migrations.PersistedGrantDb
{
    /// <inheritdoc />
    public partial class initPersistedGrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "deviceflowcodes",
                schema: "public",
                columns: table => new
                {
                    user_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    device_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deviceflowcodes", x => x.user_code);
                });

            migrationBuilder.CreateTable(
                name: "persistedgrant",
                schema: "public",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    consumed_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persistedgrant", x => x.key);
                });

            migrationBuilder.CreateIndex(
                name: "ix_deviceflowcodes_device_code",
                schema: "public",
                table: "deviceflowcodes",
                column: "device_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deviceflowcodes_expiration",
                schema: "public",
                table: "deviceflowcodes",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_persistedgrant_expiration",
                schema: "public",
                table: "persistedgrant",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_persistedgrant_subject_id_client_id_type",
                schema: "public",
                table: "persistedgrant",
                columns: new[] { "subject_id", "client_id", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_persistedgrant_subject_id_session_id_type",
                schema: "public",
                table: "persistedgrant",
                columns: new[] { "subject_id", "session_id", "type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deviceflowcodes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "persistedgrant",
                schema: "public");
        }
    }
}
