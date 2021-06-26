namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUidTextToServiceCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "UidText", c => c.String(nullable: false));
		}
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCards", "UidText");
        }
    }
}
