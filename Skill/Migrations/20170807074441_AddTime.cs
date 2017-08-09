using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skill.Migrations
{
    public partial class AddTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeginTime",
                table: "Project",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinishTime",
                table: "Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginTime",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "Project");
        }
    }
}
