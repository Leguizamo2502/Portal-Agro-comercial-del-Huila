using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormModules_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormModules_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolFormPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolFormPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Rols_RolId",
                        column: x => x.RolId,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Producers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolUsers_Rols_RolId",
                        column: x => x.RolId,
                        principalTable: "Rols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hectares = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    ProducerId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Farms_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Farms_Producers_ProducerId",
                        column: x => x.ProducerId,
                        principalTable: "Producers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FarmImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmImages_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Production = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    FarmId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Active", "CreateAt", "IsDeleted", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Frutas", null },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Hortalizas", null },
                    { 7, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Granos", null }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "Active", "CreateAt", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Amazonas" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Antioquia" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Arauca" },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Atlántico" },
                    { 5, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Bolívar" },
                    { 6, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Boyacá" },
                    { 7, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Caldas" },
                    { 8, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Caquetá" },
                    { 9, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Casanare" },
                    { 10, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cauca" },
                    { 11, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cesar" },
                    { 12, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Chocó" },
                    { 13, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Córdoba" },
                    { 14, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cundinamarca" },
                    { 15, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Guainía" },
                    { 16, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Guaviare" },
                    { 17, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Huila" },
                    { 18, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "La Guajira" },
                    { 19, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Magdalena" },
                    { 20, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Meta" },
                    { 21, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Nariño" },
                    { 22, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Norte de Santander" },
                    { 23, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Putumayo" },
                    { 24, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Quindío" },
                    { 25, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Risaralda" },
                    { 26, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "San Andrés y Providencia" },
                    { 27, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Santander" },
                    { 28, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Sucre" },
                    { 29, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Tolima" },
                    { 30, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Valle del Cauca" },
                    { 31, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vaupés" },
                    { 32, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Vichada" }
                });

            migrationBuilder.InsertData(
                table: "Forms",
                columns: new[] { "Id", "Active", "CreateAt", "Description", "IsDeleted", "Name", "Url" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de formularios", false, "Formularios", "/account/security/form" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de usuarios", false, "Usuarios", "/account/security/user" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de roles", false, "Roles", "/account/security/rol" },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de módulos", false, "Módulos", "/account/security/module" },
                    { 5, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asignación rol-usuario", false, "Rol-Usuario", "/account/security/rolUser" },
                    { 6, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asignación módulo-form", false, "Módulo-Formulario", "/account/security/formModule" },
                    { 7, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asignación R-F-P", false, "Rol-Formulario-Permiso", "/account/security/rolFormPermission" },
                    { 8, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Catálogo de permisos", false, "Permisos", "/account/security/permission" },
                    { 9, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puerta de acceso a la gestión del productor", false, "Inicio productor", "/account/producer" },
                    { 10, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resumen", false, "Resumen Productor", "/account/producer/summary" },
                    { 11, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de productos", false, "Productos", "/account/producer/management/product" },
                    { 12, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión de fincas", false, "Fincas", "/account/producer/management/farm" }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Id", "Active", "CreateAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administración de seguridad", false, "Seguridad" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parámetros del sistema", false, "Parámetros" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gestión del productor", false, "Productor" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Active", "CreateAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puede ver", false, "leer" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puede crear", false, "crear" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puede editar", false, "actualizar" },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puede eliminar", false, "eliminar" }
                });

            migrationBuilder.InsertData(
                table: "Rols",
                columns: new[] { "Id", "Active", "CreateAt", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rol con permisos administrativos", false, "Admin" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rol con permisos de usuario", false, "Consumer" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rol con permisos de Productor", false, "Producer" }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Active", "CreateAt", "IsDeleted", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cítricos", 1 },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Tropicales", 1 },
                    { 6, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Tubérculos", 5 },
                    { 8, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Café", 7 },
                    { 9, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Cacao", 7 }
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "Active", "CreateAt", "DepartmentId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Acevedo" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Agrado" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Aipe" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Algeciras" },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Altamira" },
                    { 6, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Baraya" },
                    { 7, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Campoalegre" },
                    { 8, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Colombia" },
                    { 9, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Elías" },
                    { 10, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Garzón" },
                    { 11, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Gigante" },
                    { 12, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Guadalupe" },
                    { 13, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Hobo" },
                    { 14, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Iquira" },
                    { 15, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Isnos" },
                    { 16, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "La Argentina" },
                    { 17, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "La Plata" },
                    { 18, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Nátaga" },
                    { 19, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Neiva" },
                    { 20, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Oporapa" },
                    { 21, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Paicol" },
                    { 22, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Palermo" },
                    { 23, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Palestina" },
                    { 24, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Pital" },
                    { 25, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Pitalito" },
                    { 26, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Rivera" },
                    { 27, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Saladoblanco" },
                    { 28, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "San Agustín" },
                    { 29, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Santa María" },
                    { 30, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Suaza" },
                    { 31, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Tarqui" },
                    { 32, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Tello" },
                    { 33, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Teruel" },
                    { 34, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Tesalia" },
                    { 35, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Timaná" },
                    { 36, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Villavieja" },
                    { 37, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, false, "Yaguará" }
                });

            migrationBuilder.InsertData(
                table: "FormModules",
                columns: new[] { "Id", "Active", "CreateAt", "FormId", "IsDeleted", "ModuleId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 1 },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 1 },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 1 },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 1 },
                    { 5, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, 1 },
                    { 6, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, 1 },
                    { 7, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, false, 1 },
                    { 8, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, false, 1 },
                    { 9, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 3 },
                    { 10, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, false, 3 },
                    { 11, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, false, 3 },
                    { 12, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 3 }
                });

            migrationBuilder.InsertData(
                table: "RolFormPermissions",
                columns: new[] { "Id", "Active", "CreateAt", "FormId", "IsDeleted", "PermissionId", "RolId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 1, 1 },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 2, 1 },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 3, 1 },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, false, 4, 1 },
                    { 5, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 1, 1 },
                    { 6, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 2, 1 },
                    { 7, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 3, 1 },
                    { 8, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, false, 4, 1 },
                    { 9, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 1, 1 },
                    { 10, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 2, 1 },
                    { 11, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 3, 1 },
                    { 12, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, false, 4, 1 },
                    { 13, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 1, 1 },
                    { 14, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 2, 1 },
                    { 15, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 3, 1 },
                    { 16, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, false, 4, 1 },
                    { 17, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, 1, 1 },
                    { 18, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, 2, 1 },
                    { 19, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, 3, 1 },
                    { 20, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, false, 4, 1 },
                    { 21, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, 1, 1 },
                    { 22, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, 2, 1 },
                    { 23, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, 3, 1 },
                    { 24, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, false, 4, 1 },
                    { 25, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, false, 1, 1 },
                    { 26, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, false, 2, 1 },
                    { 27, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, false, 3, 1 },
                    { 28, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, false, 4, 1 },
                    { 29, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, false, 1, 1 },
                    { 30, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, false, 2, 1 },
                    { 31, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, false, 3, 1 },
                    { 32, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, false, 4, 1 },
                    { 33, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 1, 1 },
                    { 34, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 2, 1 },
                    { 35, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 3, 1 },
                    { 36, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, false, 4, 1 },
                    { 37, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, false, 1, 1 },
                    { 38, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, false, 2, 1 },
                    { 39, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, false, 3, 1 },
                    { 40, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, false, 4, 1 },
                    { 41, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, false, 1, 1 },
                    { 42, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, false, 2, 1 },
                    { 43, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, false, 3, 1 },
                    { 44, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, false, 4, 1 },
                    { 45, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 1, 1 },
                    { 46, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 2, 1 },
                    { 47, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 3, 1 },
                    { 48, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, false, 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Active", "CreateAt", "IsDeleted", "Name", "ParentCategoryId" },
                values: new object[] { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Exóticas", 3 });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Active", "Address", "CityId", "CreateAt", "FirstName", "Identification", "IsDeleted", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, true, "Calle 1 # 1-1", 33, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Persona1", "000000000", false, "Persona1", "3000000000" },
                    { 2, true, "Carrera 10 # 20-15", 34, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Persona2", "000000001", false, "Persona2", "3000000001" },
                    { 3, true, "Avenida 3 # 5-30", 35, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Persona3", "000000002", false, "Persona3", "3000000003" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "CreateAt", "Email", "IsDeleted", "Password", "PersonId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", false, "3b612c75a7b5048a435fb6ec81e52ff92d6d795a8b5a9c17070f6a63c97a53b2", 1 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@example.com", false, "a61a8adf60038792a2cb88e670b20540a9d6c2ca204ab754fc768950e79e7d36", 2 },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "producer@example.com", false, "5cc500a2237915f8c6d906d4ea5c9632a3e0a6220d7cdffc620fe36cbbb92316", 3 }
                });

            migrationBuilder.InsertData(
                table: "Producers",
                columns: new[] { "Id", "Active", "Code", "CreateAt", "Description", "IsDeleted", "UserId" },
                values: new object[,]
                {
                    { 1, true, "PENDIENTE", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hola vendo papa", true, 3 },
                    { 2, true, "PENDIENTE", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hola vendo papa modo admin", true, 1 }
                });

            migrationBuilder.InsertData(
                table: "RolUsers",
                columns: new[] { "Id", "Active", "CreateAt", "IsDeleted", "RolId", "UserId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 1 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 1 },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, 1 },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 3, 3 },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 3 },
                    { 6, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "Farms",
                columns: new[] { "Id", "Active", "Altitude", "CityId", "CreateAt", "Hectares", "IsDeleted", "Latitude", "Longitude", "Name", "ProducerId" },
                values: new object[,]
                {
                    { 1, true, 1600.0, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4.0, false, 1200.0, 600.0, "Finca el Jardin", 1 },
                    { 2, true, 1600.0, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4.0, false, 1200.0, 600.0, "Finca el Mirador", 1 },
                    { 3, true, 1600.0, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4.0, false, 1200.0, 600.0, "Finca los Alpes", 1 },
                    { 4, true, 1600.0, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4.0, false, 1200.0, 600.0, "Finca los Lulos", 1 },
                    { 5, true, 1600.0, 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4.0, false, 1200.0, 600.0, "Finca los Primos", 2 }
                });

            migrationBuilder.InsertData(
                table: "FarmImages",
                columns: new[] { "Id", "Active", "CreateAt", "FarmId", "FileName", "ImageUrl", "IsDeleted", "PublicId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg", false, "default" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg", false, "default" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg", false, "default" },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754487968/defaultFarm_xkify3.jpg", false, "default" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Active", "CategoryId", "CreateAt", "Description", "FarmId", "IsDeleted", "Name", "Price", "Production", "Status", "Stock", "Unit" },
                values: new object[,]
                {
                    { 1, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cafe con el mejor sabor del campo", 1, false, "Cafe el sabor", 30000.0, "300 lb cada tres meses", true, 250, "lb" },
                    { 2, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cultivado sin químicos, sabor intenso", 1, false, "Café Orgánico Premium", 35000.0, "200 lb por trimestre", true, 180, "lb" },
                    { 3, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tueste medio con notas frutales", 1, false, "Café Tostado Suave", 32000.0, "150 lb cada mes", true, 120, "lb" },
                    { 4, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grano seleccionado de alta montaña", 2, false, "Café Grano Oscuro", 34000.0, "180 lb bimestral", true, 210, "lb" },
                    { 5, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cosechado a mano en clima fresco", 2, false, "Café El Mirador", 30000.0, "220 lb trimestral", true, 190, "lb" },
                    { 6, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sabor balanceado, aroma suave", 2, false, "Café Clásico de los Andes", 31000.0, "250 lb trimestral", true, 170, "lb" },
                    { 7, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mezcla selecta de granos", 3, false, "Café Supremo", 36000.0, "300 lb cada 2 meses", true, 260, "lb" },
                    { 8, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cultivo en altitudes extremas", 3, false, "Café los Alpes", 37000.0, "280 lb trimestral", true, 250, "lb" },
                    { 9, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mezcla con notas cítricas", 4, false, "Café Lulo Blend", 33000.0, "230 lb bimestral", true, 200, "lb" },
                    { 10, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tueste natural, suave al paladar", 4, false, "Café del Bosque", 30000.0, "180 lb mensual", true, 160, "lb" },
                    { 11, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grano seleccionado manualmente", 1, false, "Café Reserva Especial", 38000.0, "150 lb cada tres meses", true, 130, "lb" },
                    { 12, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cultivo bajo sombra natural", 2, false, "Café Sierra Verde", 31000.0, "200 lb cada 2 meses", true, 140, "lb" },
                    { 13, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grano joven de excelente aroma", 3, false, "Café del Amanecer", 30500.0, "160 lb mensual", true, 150, "lb" },
                    { 14, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tueste lento en horno de barro", 4, false, "Café Tostado Artesanal", 34000.0, "190 lb bimestral", true, 175, "lb" },
                    { 15, true, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mezcla suave con aroma a chocolate", 2, false, "Café con Cacao", 33000.0, "210 lb trimestral", true, 160, "lb" },
                    { 16, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sabor intenso con notas amaderadas", 3, false, "Café Gourmet del Campo", 37000.0, "280 lb bimestral", true, 210, "lb" },
                    { 17, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Proceso húmedo tradicional", 1, false, "Café Lavado", 32000.0, "190 lb mensual", true, 160, "lb" },
                    { 18, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secado al sol directamente", 4, false, "Café Natural", 31000.0, "220 lb cada 3 meses", true, 180, "lb" },
                    { 19, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Granos cultivados a 1600msnm", 2, false, "Café de Altura", 35000.0, "270 lb trimestral", true, 200, "lb" },
                    { 20, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Versión fuerte ideal para espresso", 4, false, "Café Lulo Espresso", 35500.0, "160 lb mensual", true, 190, "lb" },
                    { 21, true, 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mezcla gourmet café y cacao", 3, false, "Café Cacao Fusion", 39000.0, "240 lb trimestral", true, 150, "lb" },
                    { 22, true, 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Selección premium para exportación", 1, false, "Café de Exportación", 40000.0, "300 lb cada 2 meses", true, 220, "lb" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "Active", "CreateAt", "FileName", "ImageUrl", "IsDeleted", "ProductId", "PublicId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 1, "default" },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 2, "default" },
                    { 3, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 3, "default" },
                    { 4, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 4, "default" },
                    { 5, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 5, "default" },
                    { 6, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 6, "default" },
                    { 7, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 7, "default" },
                    { 8, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 8, "default" },
                    { 9, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 9, "default" },
                    { 10, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 10, "default" },
                    { 11, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 11, "default" },
                    { 12, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 12, "default" },
                    { 13, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 13, "default" },
                    { 14, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 14, "default" },
                    { 15, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 15, "default" },
                    { 16, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 16, "default" },
                    { 17, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 17, "default" },
                    { 18, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 18, "default" },
                    { 19, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 19, "default" },
                    { 20, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 20, "default" },
                    { 21, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 21, "default" },
                    { 22, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Imagen_Default.jpg", "https://res.cloudinary.com/djj163sc9/image/upload/v1754488119/REVERDERI-200-G_dj1poi.png", false, 22, "default" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryId",
                table: "Category",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_City_DepartmentId",
                table: "City",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmImages_FarmId",
                table: "FarmImages",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_CityId",
                table: "Farms",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_ProducerId",
                table: "Farms",
                column: "ProducerId");

            migrationBuilder.CreateIndex(
                name: "IX_FormModules_FormId",
                table: "FormModules",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FormModules_ModuleId",
                table: "FormModules",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CityId",
                table: "Persons",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Producers_UserId",
                table: "Producers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmId",
                table: "Products",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_FormId",
                table: "RolFormPermissions",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_PermissionId",
                table: "RolFormPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_RolId",
                table: "RolFormPermissions",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolUsers_RolId",
                table: "RolUsers",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolUsers_UserId",
                table: "RolUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonId",
                table: "Users",
                column: "PersonId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmImages");

            migrationBuilder.DropTable(
                name: "FormModules");

            migrationBuilder.DropTable(
                name: "PasswordResetCodes");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "RolFormPermissions");

            migrationBuilder.DropTable(
                name: "RolUsers");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Farms");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
