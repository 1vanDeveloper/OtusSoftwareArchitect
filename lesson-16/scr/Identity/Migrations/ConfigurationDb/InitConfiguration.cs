using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Identity.Migrations.ConfigurationDb
{
    /// <inheritdoc />
    public partial class InitConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "apiresource",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    allowed_access_token_signing_algorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    show_in_discovery_document = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_accessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    non_editable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiresource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "apiscope",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    show_in_discovery_document = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiscope", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    protocol_type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    require_client_secret = table.Column<bool>(type: "boolean", nullable: false),
                    client_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    client_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    logo_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    require_consent = table.Column<bool>(type: "boolean", nullable: false),
                    allow_remember_consent = table.Column<bool>(type: "boolean", nullable: false),
                    always_include_user_claims_in_id_token = table.Column<bool>(type: "boolean", nullable: false),
                    require_pkce = table.Column<bool>(type: "boolean", nullable: false),
                    allow_plain_text_pkce = table.Column<bool>(type: "boolean", nullable: false),
                    require_request_object = table.Column<bool>(type: "boolean", nullable: false),
                    allow_access_tokens_via_browser = table.Column<bool>(type: "boolean", nullable: false),
                    front_channel_logout_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    front_channel_logout_session_required = table.Column<bool>(type: "boolean", nullable: false),
                    back_channel_logout_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    back_channel_logout_session_required = table.Column<bool>(type: "boolean", nullable: false),
                    allow_offline_access = table.Column<bool>(type: "boolean", nullable: false),
                    identity_token_lifetime = table.Column<int>(type: "integer", nullable: false),
                    allowed_identity_token_signing_algorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    access_token_lifetime = table.Column<int>(type: "integer", nullable: false),
                    authorization_code_lifetime = table.Column<int>(type: "integer", nullable: false),
                    consent_lifetime = table.Column<int>(type: "integer", nullable: true),
                    absolute_refresh_token_lifetime = table.Column<int>(type: "integer", nullable: false),
                    sliding_refresh_token_lifetime = table.Column<int>(type: "integer", nullable: false),
                    refresh_token_usage = table.Column<int>(type: "integer", nullable: false),
                    update_access_token_claims_on_refresh = table.Column<bool>(type: "boolean", nullable: false),
                    refresh_token_expiration = table.Column<int>(type: "integer", nullable: false),
                    access_token_type = table.Column<int>(type: "integer", nullable: false),
                    enable_local_login = table.Column<bool>(type: "boolean", nullable: false),
                    include_jwt_id = table.Column<bool>(type: "boolean", nullable: false),
                    always_send_client_claims = table.Column<bool>(type: "boolean", nullable: false),
                    client_claims_prefix = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    pair_wise_subject_salt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_accessed = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user_sso_lifetime = table.Column<int>(type: "integer", nullable: true),
                    user_code_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    device_code_lifetime = table.Column<int>(type: "integer", nullable: false),
                    non_editable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "identityresource",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    emphasize = table.Column<bool>(type: "boolean", nullable: false),
                    show_in_discovery_document = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    non_editable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityresource", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "apiresourceclaim",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    api_resource_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiresourceclaim", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiresourceclaim_apiresource_api_resource_id",
                        column: x => x.api_resource_id,
                        principalSchema: "public",
                        principalTable: "apiresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiresourceproperty",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    api_resource_id = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiresourceproperty", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiresourceproperty_apiresource_api_resource_id",
                        column: x => x.api_resource_id,
                        principalSchema: "public",
                        principalTable: "apiresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiresourcescope",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    api_resource_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiresourcescope", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiresourcescope_apiresource_api_resource_id",
                        column: x => x.api_resource_id,
                        principalSchema: "public",
                        principalTable: "apiresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiresourcesecret",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    api_resource_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiresourcesecret", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiresourcesecret_apiresource_api_resource_id",
                        column: x => x.api_resource_id,
                        principalSchema: "public",
                        principalTable: "apiresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiscopeclaim",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scope_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiscopeclaim", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiscopeclaim_apiscope_scope_id",
                        column: x => x.scope_id,
                        principalSchema: "public",
                        principalTable: "apiscope",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "apiscopeproperty",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scope_id = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_apiscopeproperty", x => x.id);
                    table.ForeignKey(
                        name: "fk_apiscopeproperty_apiscope_scope_id",
                        column: x => x.scope_id,
                        principalSchema: "public",
                        principalTable: "apiscope",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientclaim",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientclaim", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientclaim_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientcorsorigin",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    origin = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientcorsorigin", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientcorsorigin_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientgranttype",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grant_type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientgranttype", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientgranttype_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientidprestriction",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    provider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientidprestriction", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientidprestriction_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientpostlogoutredirecturi",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    post_logout_redirect_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientpostlogoutredirecturi", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientpostlogoutredirecturi_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientproperty",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientproperty", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientproperty_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientredirecturi",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    redirect_uri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientredirecturi", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientredirecturi_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientscope",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientscope", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientscope_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clientsecret",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clientsecret", x => x.id);
                    table.ForeignKey(
                        name: "fk_clientsecret_client_client_id",
                        column: x => x.client_id,
                        principalSchema: "public",
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityresourceclaim",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identity_resource_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityresourceclaim", x => x.id);
                    table.ForeignKey(
                        name: "fk_identityresourceclaim_identityresource_identity_resource_id",
                        column: x => x.identity_resource_id,
                        principalSchema: "public",
                        principalTable: "identityresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identityresourceproperty",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identity_resource_id = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_identityresourceproperty", x => x.id);
                    table.ForeignKey(
                        name: "fk_identityresourceproperty_identityresource_identity_resource",
                        column: x => x.identity_resource_id,
                        principalSchema: "public",
                        principalTable: "identityresource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_apiresource_name",
                schema: "public",
                table: "apiresource",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_apiresourceclaim_api_resource_id",
                schema: "public",
                table: "apiresourceclaim",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_apiresourceproperty_api_resource_id",
                schema: "public",
                table: "apiresourceproperty",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_apiresourcescope_api_resource_id",
                schema: "public",
                table: "apiresourcescope",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_apiresourcesecret_api_resource_id",
                schema: "public",
                table: "apiresourcesecret",
                column: "api_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_apiscope_name",
                schema: "public",
                table: "apiscope",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_apiscopeclaim_scope_id",
                schema: "public",
                table: "apiscopeclaim",
                column: "scope_id");

            migrationBuilder.CreateIndex(
                name: "ix_apiscopeproperty_scope_id",
                schema: "public",
                table: "apiscopeproperty",
                column: "scope_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_client_id",
                schema: "public",
                table: "client",
                column: "client_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clientclaim_client_id",
                schema: "public",
                table: "clientclaim",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientcorsorigin_client_id",
                schema: "public",
                table: "clientcorsorigin",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientgranttype_client_id",
                schema: "public",
                table: "clientgranttype",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientidprestriction_client_id",
                schema: "public",
                table: "clientidprestriction",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientpostlogoutredirecturi_client_id",
                schema: "public",
                table: "clientpostlogoutredirecturi",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientproperty_client_id",
                schema: "public",
                table: "clientproperty",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientredirecturi_client_id",
                schema: "public",
                table: "clientredirecturi",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientscope_client_id",
                schema: "public",
                table: "clientscope",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_clientsecret_client_id",
                schema: "public",
                table: "clientsecret",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_identityresource_name",
                schema: "public",
                table: "identityresource",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_identityresourceclaim_identity_resource_id",
                schema: "public",
                table: "identityresourceclaim",
                column: "identity_resource_id");

            migrationBuilder.CreateIndex(
                name: "ix_identityresourceproperty_identity_resource_id",
                schema: "public",
                table: "identityresourceproperty",
                column: "identity_resource_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apiresourceclaim",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiresourceproperty",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiresourcescope",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiresourcesecret",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiscopeclaim",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiscopeproperty",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientclaim",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientcorsorigin",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientgranttype",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientidprestriction",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientpostlogoutredirecturi",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientproperty",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientredirecturi",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientscope",
                schema: "public");

            migrationBuilder.DropTable(
                name: "clientsecret",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityresourceclaim",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityresourceproperty",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiresource",
                schema: "public");

            migrationBuilder.DropTable(
                name: "apiscope",
                schema: "public");

            migrationBuilder.DropTable(
                name: "client",
                schema: "public");

            migrationBuilder.DropTable(
                name: "identityresource",
                schema: "public");
        }
    }
}
