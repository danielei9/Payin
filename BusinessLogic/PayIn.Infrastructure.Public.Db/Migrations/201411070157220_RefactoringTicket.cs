namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefactoringTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "CityName", c => c.String());
            AddColumn("dbo.Tickets", "AddressName", c => c.String());
            AddColumn("dbo.Tickets", "AddressNumber", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "AddressNumber");
            DropColumn("dbo.Tickets", "AddressName");
            DropColumn("dbo.Tickets", "CityName");
        }
    }
}
