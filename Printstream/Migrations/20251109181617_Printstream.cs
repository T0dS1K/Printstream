using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Printstream.Migrations
{
    /// <inheritdoc />
    public partial class Printstream : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address = table.Column<string>(type: "character varying(228)", maxLength: 228, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Bunch",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    bunch = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bunch", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Email",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(52)", maxLength: 52, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Email", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PersonData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsMale = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonData", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Phone",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phone", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BunchID = table.Column<int>(type: "integer", nullable: false),
                    PersonDataID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Person_Bunch_BunchID",
                        column: x => x.BunchID,
                        principalTable: "Bunch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_PersonData_PersonDataID",
                        column: x => x.PersonDataID,
                        principalTable: "PersonData",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person_Address",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "integer", nullable: false),
                    PersonID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person_Address", x => new { x.AddressID, x.PersonID });
                    table.ForeignKey(
                        name: "FK_Person_Address_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Address_Person_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person_Email",
                columns: table => new
                {
                    EmailID = table.Column<int>(type: "integer", nullable: false),
                    PersonID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person_Email", x => new { x.EmailID, x.PersonID });
                    table.ForeignKey(
                        name: "FK_Person_Email_Email_EmailID",
                        column: x => x.EmailID,
                        principalTable: "Email",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Email_Person_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person_Phone",
                columns: table => new
                {
                    PersonID = table.Column<int>(type: "integer", nullable: false),
                    PhoneID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person_Phone", x => new { x.PersonID, x.PhoneID });
                    table.ForeignKey(
                        name: "FK_Person_Phone_Person_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Person",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Person_Phone_Phone_PhoneID",
                        column: x => x.PhoneID,
                        principalTable: "Phone",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_address",
                table: "Address",
                column: "address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bunch_bunch",
                table: "Bunch",
                column: "bunch",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Email_email",
                table: "Email",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_BunchID",
                table: "Person",
                column: "BunchID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_PersonDataID",
                table: "Person",
                column: "PersonDataID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Address_PersonID",
                table: "Person_Address",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Email_PersonID",
                table: "Person_Email",
                column: "PersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Phone_PhoneID",
                table: "Person_Phone",
                column: "PhoneID");

            migrationBuilder.CreateIndex(
                name: "IX_PersonData_LastName_FirstName_MiddleName_DateOfBirth_IsMale",
                table: "PersonData",
                columns: new[] { "LastName", "FirstName", "MiddleName", "DateOfBirth", "IsMale" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phone_phone",
                table: "Phone",
                column: "phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person_Address");

            migrationBuilder.DropTable(
                name: "Person_Email");

            migrationBuilder.DropTable(
                name: "Person_Phone");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Email");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Phone");

            migrationBuilder.DropTable(
                name: "Bunch");

            migrationBuilder.DropTable(
                name: "PersonData");
        }
    }
}
