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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    whisker_length = table.Column<double>(type: "double precision", nullable: false, comment: "鬍鬚長度"),
                    loves_box = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    breed = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_good_boy = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    c1 = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "C1"),
                    c2 = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "C2")
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    @enum = table.Column<int>(name: "enum", type: "integer", nullable: true),
                    @string = table.Column<string>(name: "string", type: "character varying(100)", maxLength: 100, nullable: false),
                    @int = table.Column<int>(name: "int", type: "integer", nullable: true),
                    @long = table.Column<long>(name: "long", type: "bigint", nullable: true),
                    @decimal = table.Column<decimal>(name: "decimal", type: "numeric(5,3)", precision: 5, scale: 3, nullable: true),
                    date_only = table.Column<DateOnly>(type: "date", nullable: true),
                    date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    string_json_objects = table.Column<string>(type: "jsonb", nullable: false),
                    value_json_object = table.Column<string>(type: "jsonb", nullable: true),
                    value_json_objects = table.Column<string>(type: "jsonb", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_app_table", x => x.id);
                    table.UniqueConstraint("a_app_table_274c57", x => x.@string);
                    table.CheckConstraint("c_app_table_d5e7b8", "int > 0");
                },
                comment: "AppTable");

            migrationBuilder.CreateTable(
                name: "closure_node",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_closure_node", x => x.id);
                },
                comment: "ClosureNode");

            migrationBuilder.CreateTable(
                name: "human_head",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ulid = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_head", x => x.id);
                    table.UniqueConstraint("a_human_head_acb1a2", x => x.ulid);
                },
                comment: "HumanHead");

            migrationBuilder.CreateTable(
                name: "link_first_content",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    link_type = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    vehicle_car_type = table.Column<int>(type: "integer", nullable: false),
                    car_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    large_car_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    content = table.Column<string>(type: "jsonb", nullable: true),
                    small_car_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_vehicle_base", x => x.id);
                    table.CheckConstraint("c_vehicle_base_cb4012", "Type IN (1, 2)");
                });

            migrationBuilder.CreateTable(
                name: "closure_path",
                columns: table => new
                {
                    ancestor_id = table.Column<long>(type: "bigint", nullable: false),
                    descendant_id = table.Column<long>(type: "bigint", nullable: false),
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
                name: "human_body",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ulid = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    head_id = table.Column<string>(type: "character varying(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_body", x => x.id);
                    table.UniqueConstraint("a_human_body_acb1a2", x => x.ulid);
                    table.ForeignKey(
                        name: "f_human_body_0d4caa",
                        column: x => x.head_id,
                        principalTable: "human_head",
                        principalColumn: "ulid",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "HumanBody");

            migrationBuilder.CreateTable(
                name: "rel_link_first_sub_content",
                columns: table => new
                {
                    link_first_content_id = table.Column<long>(type: "bigint", nullable: false),
                    link_sub_content_id = table.Column<long>(type: "bigint", nullable: false)
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
                    link_second_content_id = table.Column<long>(type: "bigint", nullable: false),
                    link_sub_content_id = table.Column<long>(type: "bigint", nullable: false)
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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reference_type = table.Column<int>(type: "integer", nullable: false),
                    reference_id = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_subject_content", x => x.id);
                    table.ForeignKey(
                        name: "d_subject_first_content_reference_id",
                        column: x => x.reference_id,
                        principalTable: "subject_first",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "d_subject_second_content_reference_id",
                        column: x => x.reference_id,
                        principalTable: "subject_second",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "human_limb",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ulid = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    color = table.Column<int>(type: "integer", nullable: false),
                    check_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    remark = table.Column<string>(type: "text", nullable: true),
                    body_id = table.Column<string>(type: "character varying(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_human_limb", x => x.id);
                    table.ForeignKey(
                        name: "f_human_limb_394eba",
                        column: x => x.body_id,
                        principalTable: "human_body",
                        principalColumn: "ulid",
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
                name: "i_app_table_274c57",
                table: "app_table",
                column: "string",
                unique: true,
                filter: "string IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "i_closure_path_b1e295",
                table: "closure_path",
                column: "descendant_id");

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
                name: "closure_path");

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
                name: "closure_node");

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
