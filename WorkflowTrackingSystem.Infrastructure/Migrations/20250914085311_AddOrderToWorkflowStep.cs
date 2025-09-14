using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkflowTrackingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToWorkflowStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WorkflowSteps",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "WorkflowSteps");
        }
    }
}
