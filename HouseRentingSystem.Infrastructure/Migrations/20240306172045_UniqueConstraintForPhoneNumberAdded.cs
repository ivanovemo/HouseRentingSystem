using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class UniqueConstraintForPhoneNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "290b6298-ee7c-4763-8f93-651ec50b5a45", "AQAAAAEAACcQAAAAEKbuog1kMx4/FwJCG63dIDCz26Ad9tgVmo8hDrQOl2Soj869IzOUJESYY+eeFSKrfw==", "4813cf9b-c530-40fd-9806-37aa560abc76" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "083264fc-8d09-4085-b83d-0fcc5f81c843", "AQAAAAEAACcQAAAAEJYuYeEtz5WglMfz2BCco3QcfJMrJ9HFrmtlIU2vDJOKkR6YuiPBiM6r1Li6kyD4gQ==", "b4d7c12a-26eb-4af9-a9dc-c43b7511a198" });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_PhoneNumber",
                table: "Agents",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Agents_PhoneNumber",
                table: "Agents");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a80622d5-06eb-4990-9b2a-0299cdcd8c85", "AQAAAAEAACcQAAAAELQrwIz7uxgRZQjhrcKddOFWlGElFBeasgO8W5/SP+yLC7UCl5VyveUunR5vTH5jUg==", "277da366-443e-4905-b0f1-99580d5dfda7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b47768f9-e2a8-43f8-abd1-2f7a0944afb3", "AQAAAAEAACcQAAAAEBODuj9o+BAVmpa0WYTfmkuRbwr4WC4JdIkhZyktf2764D517hIgPZ6P3fqfE6fktw==", "e002c6ac-41b1-4458-9777-439c8c6366a3" });
        }
    }
}
