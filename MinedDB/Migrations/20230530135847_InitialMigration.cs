using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Mined.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: true),
                    MainCategoryNatoEng = table.Column<string>(type: "longtext", nullable: true),
                    SubCategoryNatoEng = table.Column<string>(type: "longtext", nullable: true),
                    MainCategoryUsEng = table.Column<string>(type: "longtext", nullable: true),
                    SubCategoryUsEng = table.Column<string>(type: "longtext", nullable: true),
                    MainCategoryNatoNl = table.Column<string>(type: "longtext", nullable: true),
                    SubCategoryNatoNl = table.Column<string>(type: "longtext", nullable: true),
                    MainCategoryUsNl = table.Column<string>(type: "longtext", nullable: true),
                    SubCategoryUsNl = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    PasswordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserPassword = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.PasswordId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Payloads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AbbreviationRussian = table.Column<string>(type: "longtext", nullable: true),
                    AbbreviationNato = table.Column<string>(type: "longtext", nullable: true),
                    PayloadTypeEng = table.Column<string>(type: "longtext", nullable: true),
                    PayloadTypeNl = table.Column<string>(type: "longtext", nullable: true),
                    ChemicalPayload = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payloads", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nickname = table.Column<string>(type: "longtext", nullable: true),
                    NumberOfMistakes = table.Column<int>(type: "int", nullable: true),
                    UxoMistakes = table.Column<string>(type: "longtext", nullable: true),
                    PlayerScore = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Uxos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UxoName = table.Column<string>(type: "longtext", nullable: true),
                    CategoryName = table.Column<string>(type: "longtext", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    NameNato = table.Column<string>(type: "longtext", nullable: true),
                    NameRussian = table.Column<string>(type: "longtext", nullable: true),
                    NameOrigin = table.Column<string>(type: "longtext", nullable: true),
                    NickName = table.Column<string>(type: "longtext", nullable: true),
                    Dod_Code = table.Column<string>(type: "longtext", nullable: true),
                    Nato_Code = table.Column<string>(type: "longtext", nullable: true),
                    description_Nl = table.Column<string>(type: "longtext", nullable: true),
                    description_Eng = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uxos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uxos_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(type: "longtext", nullable: true),
                    PasswordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Passwords_PasswordId",
                        column: x => x.PasswordId,
                        principalTable: "Passwords",
                        principalColumn: "PasswordId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UxoName = table.Column<string>(type: "longtext", nullable: true),
                    UxoId = table.Column<int>(type: "int", nullable: false),
                    UxoImage = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Uxos_UxoId",
                        column: x => x.UxoId,
                        principalTable: "Uxos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UxoPayloads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UxoName = table.Column<int>(type: "int", nullable: true),
                    UxoId = table.Column<int>(type: "int", nullable: false),
                    PayloadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UxoPayloads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UxoPayloads_Payloads_PayloadId",
                        column: x => x.PayloadId,
                        principalTable: "Payloads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UxoPayloads_Uxos_UxoId",
                        column: x => x.UxoId,
                        principalTable: "Uxos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UxoId",
                table: "Images",
                column: "UxoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordId",
                table: "Users",
                column: "PasswordId");

            migrationBuilder.CreateIndex(
                name: "IX_UxoPayloads_PayloadId",
                table: "UxoPayloads",
                column: "PayloadId");

            migrationBuilder.CreateIndex(
                name: "IX_UxoPayloads_UxoId",
                table: "UxoPayloads",
                column: "UxoId");

            migrationBuilder.CreateIndex(
                name: "IX_Uxos_CategoryId",
                table: "Uxos",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UxoPayloads");

            migrationBuilder.DropTable(
                name: "Passwords");

            migrationBuilder.DropTable(
                name: "Payloads");

            migrationBuilder.DropTable(
                name: "Uxos");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
