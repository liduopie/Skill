using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Skill.Migrations
{
    public partial class Addo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BeginTime",
                table: "ProjectPartakePerson",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinishTime",
                table: "ProjectPartakePerson",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginTime",
                table: "ProjectPartakePerson");

            migrationBuilder.DropColumn(
                name: "FinishTime",
                table: "ProjectPartakePerson");
        }
    }
}
