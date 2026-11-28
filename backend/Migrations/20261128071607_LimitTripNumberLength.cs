
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class LimitTripNumberLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Преобразуем старые длинные номера,
            // используя уникальные идентификаторы поездок.
            migrationBuilder.Sql(@"
                UPDATE trips
                SET number = 'LEGACY-' || id::text
                WHERE char_length(number) > 20;
            ");

            // Ограничиваем длину номера поездки.
            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "trips",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "number",
                table: "trips",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);
        }
    }
}
