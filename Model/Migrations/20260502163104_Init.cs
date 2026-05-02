using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_cat",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    whisker_length = table.Column<double>(type: "double precision", nullable: false, comment: "鬍鬚長度"),
                    loves_box = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    flag = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_animal_cat", x => x.id);
                },
                comment: "AnimalCat");

            migrationBuilder.CreateTable(
                name: "animal_dog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    breed = table.Column<string>(type: "text", maxLength: 50, nullable: false),
                    is_good_boy = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    flag = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_animal_dog", x => x.id);
                },
                comment: "AnimalDog");

            migrationBuilder.CreateTable(
                name: "app_index",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    c1 = table.Column<string>(type: "text", maxLength: 10, nullable: false, defaultValue: "C1"),
                    c2 = table.Column<string>(type: "text", unicode: false, maxLength: 20, nullable: false, defaultValue: "C2")
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_app_index", x => x.id);
                },
                comment: "AppIndex");

            migrationBuilder.CreateTable(
                name: "app_table",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    @enum = table.Column<int>(name: "enum", type: "integer", nullable: true),
                    @string = table.Column<string>(name: "string", type: "text", maxLength: 100, nullable: true),
                    @int = table.Column<int>(name: "int", type: "integer", nullable: true),
                    @long = table.Column<long>(name: "long", type: "bigint", nullable: true),
                    @decimal = table.Column<decimal>(name: "decimal", type: "numeric(5,3)", precision: 5, scale: 3, nullable: true),
                    date_only = table.Column<DateOnly>(type: "date", nullable: true),
                    date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    any_json_string = table.Column<string>(type: "jsonb", nullable: true),
                    string_array = table.Column<List<string>>(type: "text[]", nullable: false, defaultValue: new List<string>()),
                    guid_array = table.Column<List<Guid>>(type: "uuid[]", nullable: false, defaultValue: new List<Guid>()),
                    value_json_object = table.Column<string>(type: "jsonb", nullable: true),
                    value_json_objects = table.Column<string>(type: "jsonb", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_app_table", x => x.id);
                    table.CheckConstraint("c_app_table_d5e7b8", "int > 0");
                },
                comment: "AppTable");

            migrationBuilder.CreateTable(
                name: "asp_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    security_stamp = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    description = table.Column<string>(type: "text", maxLength: 128, nullable: false),
                    user_name = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", maxLength: 512, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    phone_number = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "closure_node",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_closure_node", x => x.id);
                },
                comment: "ClosureNode");

            migrationBuilder.CreateTable(
                name: "family_parent",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "created_at"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "updated_at")
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_family_parent", x => x.id);
                },
                comment: "FamilyParent");

            migrationBuilder.CreateTable(
                name: "human_head",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ulid = table.Column<string>(type: "text", unicode: false, maxLength: 26, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_head", x => x.id);
                },
                comment: "HumanHead");

            migrationBuilder.CreateTable(
                name: "link_first_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_link_first_content", x => x.id);
                },
                comment: "LinkFirstContent");

            migrationBuilder.CreateTable(
                name: "link_second_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_link_second_content", x => x.id);
                },
                comment: "LinkSecondContent");

            migrationBuilder.CreateTable(
                name: "link_sub_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    link_type = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", maxLength: 100, nullable: false),
                    first = table.Column<string>(type: "text", nullable: true),
                    second = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_link_sub_content", x => x.id);
                },
                comment: "LinkSubContent");

            migrationBuilder.CreateTable(
                name: "subject_first",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_subject_first", x => x.id);
                },
                comment: "SubjectFirst");

            migrationBuilder.CreateTable(
                name: "subject_second",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_subject_second", x => x.id);
                },
                comment: "SubjectSecond");

            migrationBuilder.CreateTable(
                name: "vehicle_base",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    vehicle_car_type = table.Column<int>(type: "integer", nullable: false),
                    car_name = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    large_car_name = table.Column<string>(type: "text", maxLength: 50, nullable: true),
                    content = table.Column<string>(type: "jsonb", nullable: true),
                    small_car_name = table.Column<string>(type: "text", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_vehicle_base", x => x.id);
                    table.CheckConstraint("c_vehicle_base_cb4012", "Type IN (1, 2)");
                });

            migrationBuilder.CreateTable(
                name: "asp_role_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    claim_value = table.Column<string>(type: "text", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_role_claim", x => x.id);
                    table.ForeignKey(
                        name: "f_asp_role_claim_69fd8d",
                        column: x => x.role_id,
                        principalTable: "asp_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_user_claim",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_type = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    claim_value = table.Column<string>(type: "text", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_user_claim", x => x.id);
                    table.ForeignKey(
                        name: "f_asp_user_claim_8934e3",
                        column: x => x.user_id,
                        principalTable: "asp_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_user_login",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    provider_key = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", maxLength: 256, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_user_login", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "f_asp_user_login_8934e3",
                        column: x => x.user_id,
                        principalTable: "asp_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_user_role",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_user_role", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "f_asp_user_role_69fd8d",
                        column: x => x.role_id,
                        principalTable: "asp_role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_asp_user_role_8934e3",
                        column: x => x.user_id,
                        principalTable: "asp_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_user_token",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    login_provider = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 256, nullable: false),
                    value = table.Column<string>(type: "text", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_asp_user_token", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "f_asp_user_token_8934e3",
                        column: x => x.user_id,
                        principalTable: "asp_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "closure_path",
                columns: table => new
                {
                    ancestor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    descendant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    depth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_closure_path", x => new { x.ancestor_id, x.descendant_id });
                    table.ForeignKey(
                        name: "f_closure_path_b1e295",
                        column: x => x.descendant_id,
                        principalTable: "closure_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_closure_path_e210a0",
                        column: x => x.ancestor_id,
                        principalTable: "closure_node",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "ClosurePath");

            migrationBuilder.CreateTable(
                name: "family_child1",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "created_at"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "updated_at"),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_family_child1", x => x.id);
                    table.ForeignKey(
                        name: "f_family_child1_6fa56f",
                        column: x => x.parent_id,
                        principalTable: "family_parent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "FamilyChild1");

            migrationBuilder.CreateTable(
                name: "family_child2",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "created_at"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "updated_at"),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_family_child2", x => x.id);
                    table.ForeignKey(
                        name: "f_family_child2_6fa56f",
                        column: x => x.parent_id,
                        principalTable: "family_parent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "FamilyChild2");

            migrationBuilder.CreateTable(
                name: "human_body",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ulid = table.Column<string>(type: "text", unicode: false, maxLength: 26, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    head_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_body", x => x.id);
                    table.ForeignKey(
                        name: "f_human_body_0d4caa",
                        column: x => x.head_id,
                        principalTable: "human_head",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "HumanBody");

            migrationBuilder.CreateTable(
                name: "rel_link_first_sub_content",
                columns: table => new
                {
                    link_first_content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    link_sub_content_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_rel_link_first_sub_content", x => new { x.link_first_content_id, x.link_sub_content_id });
                    table.ForeignKey(
                        name: "f_rel_link_first_sub_content_9c6c37",
                        column: x => x.link_first_content_id,
                        principalTable: "link_first_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_rel_link_first_sub_content_d5f4dd",
                        column: x => x.link_sub_content_id,
                        principalTable: "link_sub_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rel_link_second_sub_content",
                columns: table => new
                {
                    link_second_content_id = table.Column<Guid>(type: "uuid", nullable: false),
                    link_sub_content_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_rel_link_second_sub_content", x => new { x.link_second_content_id, x.link_sub_content_id });
                    table.ForeignKey(
                        name: "f_rel_link_second_sub_content_8fa616",
                        column: x => x.link_second_content_id,
                        principalTable: "link_second_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_rel_link_second_sub_content_d5f4dd",
                        column: x => x.link_sub_content_id,
                        principalTable: "link_sub_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reference_type = table.Column<int>(type: "integer", nullable: false),
                    reference_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_subject_content", x => x.id);
                    table.ForeignKey(
                        name: "d_subject_first_content_3cc620",
                        column: x => x.reference_id,
                        principalTable: "subject_first",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "d_subject_second_content_3cc620",
                        column: x => x.reference_id,
                        principalTable: "subject_second",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "human_limb",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ulid = table.Column<string>(type: "text", unicode: false, maxLength: 26, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    body_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_limb", x => x.id);
                    table.ForeignKey(
                        name: "f_human_limb_394eba",
                        column: x => x.body_id,
                        principalTable: "human_body",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "HumanLimb");

            migrationBuilder.CreateIndex(
                name: "i_app_index_4d1f45",
                table: "app_index",
                column: "c1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_app_index_d3bba8",
                table: "app_index",
                columns: new[] { "c1", "c2" });

            migrationBuilder.CreateIndex(
                name: "i_app_table_119587",
                table: "app_table",
                column: "string_array")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "i_app_table_274c57",
                table: "app_table",
                column: "string",
                unique: true,
                filter: "string IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "i_app_table_36bbf3",
                table: "app_table",
                column: "value_json_objects")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "i_app_table_43f885",
                table: "app_table",
                column: "guid_array")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "i_app_table_bc7bf3",
                table: "app_table",
                column: "value_json_object")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "i_asp_role_51e4ef",
                table: "asp_role",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_asp_role_claim_69fd8d",
                table: "asp_role_claim",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "i_asp_user_2004df",
                table: "asp_user",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_asp_user_2182de",
                table: "asp_user",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "i_asp_user_claim_8934e3",
                table: "asp_user_claim",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_asp_user_login_8934e3",
                table: "asp_user_login",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_asp_user_role_69fd8d",
                table: "asp_user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "i_closure_path_b1e295",
                table: "closure_path",
                column: "descendant_id");

            migrationBuilder.CreateIndex(
                name: "i_family_child1_6fa56f",
                table: "family_child1",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "i_family_child2_6fa56f",
                table: "family_child2",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "i_human_body_0d4caa",
                table: "human_body",
                column: "head_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_human_limb_394eba",
                table: "human_limb",
                column: "body_id");

            migrationBuilder.CreateIndex(
                name: "i_rel_link_first_sub_content_d5f4dd",
                table: "rel_link_first_sub_content",
                column: "link_sub_content_id");

            migrationBuilder.CreateIndex(
                name: "i_rel_link_second_sub_content_d5f4dd",
                table: "rel_link_second_sub_content",
                column: "link_sub_content_id");

            migrationBuilder.CreateIndex(
                name: "i_subject_content_416b18",
                table: "subject_content",
                columns: new[] { "reference_id", "reference_type" });

            migrationBuilder.CreateIndex(
                name: "i_vehicle_base_682a0e",
                table: "vehicle_base",
                column: "type");

            migrationBuilder.CreateScripts();
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_cat");

            migrationBuilder.DropTable(
                name: "animal_dog");

            migrationBuilder.DropTable(
                name: "app_index");

            migrationBuilder.DropTable(
                name: "app_table");

            migrationBuilder.DropTable(
                name: "asp_role_claim");

            migrationBuilder.DropTable(
                name: "asp_user_claim");

            migrationBuilder.DropTable(
                name: "asp_user_login");

            migrationBuilder.DropTable(
                name: "asp_user_role");

            migrationBuilder.DropTable(
                name: "asp_user_token");

            migrationBuilder.DropTable(
                name: "closure_path");

            migrationBuilder.DropTable(
                name: "family_child1");

            migrationBuilder.DropTable(
                name: "family_child2");

            migrationBuilder.DropTable(
                name: "human_limb");

            migrationBuilder.DropTable(
                name: "rel_link_first_sub_content");

            migrationBuilder.DropTable(
                name: "rel_link_second_sub_content");

            migrationBuilder.DropTable(
                name: "subject_content");

            migrationBuilder.DropTable(
                name: "vehicle_base");

            migrationBuilder.DropTable(
                name: "asp_role");

            migrationBuilder.DropTable(
                name: "asp_user");

            migrationBuilder.DropTable(
                name: "closure_node");

            migrationBuilder.DropTable(
                name: "family_parent");

            migrationBuilder.DropTable(
                name: "human_body");

            migrationBuilder.DropTable(
                name: "link_first_content");

            migrationBuilder.DropTable(
                name: "link_second_content");

            migrationBuilder.DropTable(
                name: "link_sub_content");

            migrationBuilder.DropTable(
                name: "subject_first");

            migrationBuilder.DropTable(
                name: "subject_second");

            migrationBuilder.DropTable(
                name: "human_head");
        }
    }
}
