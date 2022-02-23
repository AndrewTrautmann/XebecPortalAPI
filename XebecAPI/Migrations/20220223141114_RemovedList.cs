﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace XebecAPI.Migrations
{
    public partial class RemovedList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationPhases_ApplicationPhases_ApplicationPhaseId",
                table: "ApplicationPhases");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationPhases_ApplicationPhaseId",
                table: "ApplicationPhases");

            migrationBuilder.DropColumn(
                name: "ApplicationPhaseId",
                table: "ApplicationPhases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationPhaseId",
                table: "ApplicationPhases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationPhases_ApplicationPhaseId",
                table: "ApplicationPhases",
                column: "ApplicationPhaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationPhases_ApplicationPhases_ApplicationPhaseId",
                table: "ApplicationPhases",
                column: "ApplicationPhaseId",
                principalTable: "ApplicationPhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
