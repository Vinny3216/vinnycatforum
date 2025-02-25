using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vinnycatforum.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFilenameToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilename",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilename",
                table: "Comment");
        }
    }
}
