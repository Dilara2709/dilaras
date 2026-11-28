using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AlignDatabaseConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_carriages_carriage_id",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_trips_users_created_by_user_id",
                table: "trips");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "trips",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_users_login",
                table: "users",
                column: "login",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_users_role",
                table: "users",
                sql: "role IN ('passenger', 'conductor', 'admin')");

            migrationBuilder.CreateIndex(
                name: "IX_trips_created_at",
                table: "trips",
                column: "created_at",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_trips_number",
                table: "trips",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trips_status",
                table: "trips",
                column: "status");

            migrationBuilder.AddCheckConstraint(
                name: "CK_trips_status",
                table: "trips",
                sql: "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_carriages_carriage_id",
                table: "trips",
                column: "carriage_id",
                principalTable: "carriages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_users_created_by_user_id",
                table: "trips",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trips_carriages_carriage_id",
                table: "trips");

            migrationBuilder.DropForeignKey(
                name: "FK_trips_users_created_by_user_id",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "IX_users_login",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_users_role",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_trips_created_at",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "IX_trips_number",
                table: "trips");

            migrationBuilder.DropIndex(
                name: "IX_trips_status",
                table: "trips");

            migrationBuilder.DropCheckConstraint(
                name: "CK_trips_status",
                table: "trips");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "trips",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AddForeignKey(
                name: "FK_trips_carriages_carriage_id",
                table: "trips",
                column: "carriage_id",
                principalTable: "carriages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_trips_users_created_by_user_id",
                table: "trips",
                column: "created_by_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
