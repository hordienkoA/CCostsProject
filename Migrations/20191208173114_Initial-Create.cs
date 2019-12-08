using CConstsProject.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCostsProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(

                name: "Email",
                table: "Users",
                nullable: false,
                defaultValue: ""
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
