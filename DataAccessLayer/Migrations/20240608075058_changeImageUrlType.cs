using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class changeImageUrlType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RequestedReport",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Step 1: Create a temporary column for the varbinary data
            migrationBuilder.AddColumn<byte[]>(
                name: "ReportUrlTemp",
                table: "RequestedReport",
                type: "varbinary(max)",
                nullable: true);

            // Step 2: Convert the existing nvarchar data to varbinary and update the temporary column
            migrationBuilder.Sql(@"
                UPDATE RequestedReport
                SET ReportUrlTemp = CONVERT(varbinary(max), ReportUrl)
            ");

            // Step 3: Drop the old nvarchar column
            migrationBuilder.DropColumn(
                name: "ReportUrl",
                table: "RequestedReport");

            // Step 4: Rename the temporary column to the original name
            migrationBuilder.RenameColumn(
                name: "ReportUrlTemp",
                table: "RequestedReport",
                newName: "ReportUrl");

            migrationBuilder.AlterColumn<string>(
                name: "DateRange",
                table: "RequestedReport",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RequestedReport",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportUrl",
                table: "RequestedReport",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
            
            migrationBuilder.AlterColumn<string>(
                name: "DateRange",
                table: "RequestedReport",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Step 1: Create a temporary column for the nvarchar data
            migrationBuilder.AddColumn<string>(
                name: "ReportUrlTemp",
                table: "RequestedReport",
                type: "nvarchar(max)",
                nullable: true);

            //// Step 2: Convert the varbinary data back to nvarchar and update the temporary column
            //migrationBuilder.Sql(@"
            //    UPDATE RequestedReport
            //    SET ReportUrlTemp = CONVERT(nvarchar(max), ReportUrl)
            //");

            // Step 3: Drop the new varbinary column
            migrationBuilder.DropColumn(
                name: "ReportUrl",
                table: "RequestedReport");

            // Step 4: Rename the temporary column back to the original name
            migrationBuilder.RenameColumn(
                name: "ReportUrlTemp",
                table: "RequestedReport",
                newName: "ReportUrl");
        }
    }
}
