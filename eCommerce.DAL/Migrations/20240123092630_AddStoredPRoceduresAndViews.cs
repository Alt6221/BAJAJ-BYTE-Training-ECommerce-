using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredPRoceduresAndViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var SP1 = @"CREATE PROCEDURE GenerateNewCart
        (
            @p_CustomerId INT,
            @p_CartId INT OUT
        ) 
	
        AS
        BEGIN
            Insert into Carts(CustomerId,CartDate) Values (@p_CustomerId,GETDATE());
            SELECT @p_CartId = SCOPE_IDENTITY();
        END";
            migrationBuilder.Sql(SP1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var DropGenerateCartSp = @"DROP PROCEDURE GenerateNewCart";
            migrationBuilder.Sql(DropGenerateCartSp);
        }
    }
}
