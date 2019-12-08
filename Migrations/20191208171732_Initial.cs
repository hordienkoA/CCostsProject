using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCostsProject.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Families",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AdditionalInformation = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Families", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Items",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        AvarageCost = table.Column<double>(nullable: false),
            //        Type = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Items", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TaskManagers",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Name = table.Column<string>(nullable: true),
            //        DueDate = table.Column<DateTime>(nullable: false),
            //        StartDate = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TaskManagers", x => x.Id);
            //    });

            migrationBuilder.AddColumn<string>(

                 name: "Email",
                 table: "Users",
                 nullable: false,
                 defaultValue: ""
                 );
            //migrationBuilder.CreateTable(
            //    name: "Tasks",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        True_rule = table.Column<string>(nullable: true),
            //        False_rule = table.Column<string>(nullable: true),
            //        TaskManagerId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tasks", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Tasks_TaskManagers_TaskManagerId",
            //            column: x => x.TaskManagerId,
            //            principalTable: "TaskManagers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Incomes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        WorkType = table.Column<string>(nullable: false),
            //        Money = table.Column<double>(nullable: false),
            //        Date = table.Column<DateTime>(nullable: false),
            //        UserId = table.Column<int>(nullable: true)
            //    },
            //constraints: table =>
            //{
            //    table.PrimaryKey("PK_Incomes", x => x.Id);
            //    table.ForeignKey(
            //        name: "FK_Incomes_Users_UserId",
            //        column: x => x.UserId,
            //        principalTable: "Users",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);
            //});

            //migrationBuilder.CreateTable(
            //    name: "Outgos",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Money = table.Column<double>(nullable: false),
            //        Date = table.Column<DateTime>(nullable: false),
            //        ItemId = table.Column<int>(nullable: true),
            //        UserId = table.Column<int>(nullable: true)
            //    },
            //constraints: table =>
            //{
            //    table.PrimaryKey("PK_Outgos", x => x.Id);
            //    table.ForeignKey(
            //        name: "FK_Outgos_Items_ItemId",
            //        column: x => x.ItemId,
            //        principalTable: "Items",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);
            //    table.ForeignKey(
            //        name: "FK_Outgos_Users_UserId",
            //        column: x => x.UserId,
            //        principalTable: "Users",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);
            //});

            //migrationBuilder.CreateIndex(
            //    name: "IX_Incomes_UserId",
            //    table: "Incomes",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Outgos_ItemId",
            //    table: "Outgos",
            //    column: "ItemId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Outgos_UserId",
            //    table: "Outgos",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tasks_TaskManagerId",
            //    table: "Tasks",
            //    column: "TaskManagerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_FamilyId",
            //    table: "Users",
            //    column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Incomes");

            //migrationBuilder.DropTable(
            //    name: "Outgos");

            //migrationBuilder.DropTable(
            //    name: "Tasks");

            //migrationBuilder.DropTable(
            //    name: "Items");

            migrationBuilder.DropColumn("Email", "Users");

            //migrationBuilder.DropTable(
            //    name: "TaskManagers");

            //migrationBuilder.DropTable(
            //    name: "Families");
        }
    }
}
