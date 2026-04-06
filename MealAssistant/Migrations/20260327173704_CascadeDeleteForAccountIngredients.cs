using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealAssistant.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForAccountIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountIngredients_Accounts_AccountId",
                table: "AccountIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountIngredients_Ingredients_IngredientId",
                table: "AccountIngredients");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountIngredients_Accounts_AccountId",
                table: "AccountIngredients",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountIngredients_Ingredients_IngredientId",
                table: "AccountIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountIngredients_Accounts_AccountId",
                table: "AccountIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountIngredients_Ingredients_IngredientId",
                table: "AccountIngredients");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountIngredients_Accounts_AccountId",
                table: "AccountIngredients",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountIngredients_Ingredients_IngredientId",
                table: "AccountIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
