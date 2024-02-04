using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCartStoredProcedureandViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			var SP2 = @"CREATE PROCEDURE RemoveCartDetails
            (
              @p_CartId INT
            )
            AS
            BEGIN
                  DELETE FROM Carts WHERE CartId = @p_CartId;
                  END";
			migrationBuilder.Sql(SP2);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			var DropRemoveCartDetailsSp = @"DROP PROCEDURE RemoveCartDetails";
			migrationBuilder.Sql(DropRemoveCartDetailsSp);
		}
    }
}
