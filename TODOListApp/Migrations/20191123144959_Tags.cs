using Microsoft.EntityFrameworkCore.Migrations;

namespace TODOListApp.Migrations
{
    public partial class Tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "TodoTags",
                columns: table => new
                {
                    TagName = table.Column<string>(nullable: false),
                    TodoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTags", x => new { x.TodoId, x.TagName });
                    table.ForeignKey(
                        name: "FK_TodoTags_Tags_TagName",
                        column: x => x.TagName,
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TodoTags_TodoItems_TodoId",
                        column: x => x.TodoId,
                        principalTable: "TodoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoTags_TagName",
                table: "TodoTags",
                column: "TagName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
