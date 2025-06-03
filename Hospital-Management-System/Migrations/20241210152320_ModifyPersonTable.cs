using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hospital_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPersonTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Address", "BirthDate", "Email", "Gender", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "ryk", new DateOnly(1990, 1, 1), "alyan@gmail.com", "M", "Alyan", "1234567890" },
                    { 2, "A-Block ryk", new DateOnly(2002, 1, 1), "ali@gmail.com", "M", "Ali", "12345677790" },
                    { 3, "B-Block ryk", new DateOnly(2003, 1, 1), "zafar@gmail.com", "M", "zafar", "03045677790" }
                });
        }
    }
}
