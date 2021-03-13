using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthRazor.Persistence.Migrations
{
    public partial class Initial_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AuthUsers",
                columns: new[] { "Id", "Email", "Password", "UserRole" },
                values: new object[] { 1, "admin@htl.at", "78yFrayVwYcgAO4k1oGLqioKZDMhSToo2YvfG4MybGg=fc2dcbe11501936f9cb9ba75aad63ac1", "Administrator" });

            migrationBuilder.InsertData(
                table: "AuthUsers",
                columns: new[] { "Id", "Email", "Password", "UserRole" },
                values: new object[] { 2, "user@htl.at", "ycNT4ybVAbOGBOVVTS+yrEQp/rfyFHE/Vh5vqb+wgg8=87548afff9f75927b93d4f4c48d9b38c", "User" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthUsers");
        }
    }
}
