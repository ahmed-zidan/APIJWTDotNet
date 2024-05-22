using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace APIJWTDotNet.Migrations
{
    public partial class seedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {
                    "Id",
                    "Name",
                    "NormalizedName",
                    "ConcurrencyStamp"
                },
                values: new object[]
                {
                    Guid.NewGuid().ToString(),"Admin" ,"Admin".ToString(),Guid.NewGuid().ToString()
                }
                );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {"Id",
                    "Name",
                    "NormalizedName",
                    "ConcurrencyStamp"},
                values: new object[]
                {
                    Guid.NewGuid().ToString(),"User" ,"User".ToString(),Guid.NewGuid().ToString()
                }

                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [AspNetRoles]");
        }
    }
}
