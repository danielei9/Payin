namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("security.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("security.AspNetUsers", "LastName", c => c.String(nullable: false));
            AddColumn("security.AspNetUsers", "TaxNumber", c => c.String(nullable: false));
            AddColumn("security.AspNetUsers", "TaxName", c => c.String(nullable: false));
            AddColumn("security.AspNetUsers", "TaxAddress", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("security.AspNetUsers", "TaxAddress");
            DropColumn("security.AspNetUsers", "TaxName");
            DropColumn("security.AspNetUsers", "TaxNumber");
            DropColumn("security.AspNetUsers", "LastName");
            DropColumn("security.AspNetUsers", "FirstName");
        }
    }
}
