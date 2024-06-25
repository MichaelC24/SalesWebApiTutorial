using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebApiTutorial.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration //migrations keeps the database in sync with classes to re sync them do (add-migration "init")
                                          //when you create a migration make sure to go through the below methods to make sure they are doing everything you want them to do

    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) // up lets you add stuff to the database
        {
            migrationBuilder.CreateTable(
                name: "Customers", //table name
                columns: table => new 
                {
                    Id = table.Column<int>(type: "int", nullable: false) //everything in () is sql
                        .Annotation("SqlServer:Identity", "1, 1"), //this itterates the primary key column by 1
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Sales = table.Column<decimal>(type: "decimal(11,2)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_Customers", x => x.Id); // set the ID Column up as the primary key
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) // reverses what ever the up does
        {
            migrationBuilder.DropTable( //drops the table 
                name: "Customers");
        }
    }
}
