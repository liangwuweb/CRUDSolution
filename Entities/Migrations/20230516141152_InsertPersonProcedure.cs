using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPersonProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @" 
                    CREATE PROCEDURE [dbo].[InsertPerson]
                    (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(max), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryID uniqueidentifier, @Address nvarchar(200), @ReceiveNewsLetters bit, @TIN nvarchar(max))
                    AS BEGIN
                        INSERT INTO [dbo].[Persons](PersonId, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TIN) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters, @TIN)
                    END
               ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @" 
                    DROP PROCEDURE [dbo].[InsertPerson]
               ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
