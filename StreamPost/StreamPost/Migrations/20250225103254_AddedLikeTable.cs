using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamPost.Migrations
{
    /// <inheritdoc />
    public partial class AddedLikeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    LikeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.LikeID);
                    table.ForeignKey(
                        name: "FK_likes_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_likes_posts_PostID",
                        column: x => x.PostID,
                        principalTable: "posts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_likes_Id",
                table: "likes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_PostID",
                table: "likes",
                column: "PostID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");
        }
    }
}
