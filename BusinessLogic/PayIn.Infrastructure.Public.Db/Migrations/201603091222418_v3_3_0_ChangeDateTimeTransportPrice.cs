namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_ChangeDateTimeTransportPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "Start", c => c.DateTime(nullable: false));
            AddColumn("dbo.TransportPrices", "End", c => c.DateTime(nullable: false));
            DropColumn("dbo.TransportPrices", "Start_Value");
            DropColumn("dbo.TransportPrices", "End_Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransportPrices", "End_Value", c => c.DateTime(nullable: false));
            AddColumn("dbo.TransportPrices", "Start_Value", c => c.DateTime(nullable: false));
            DropColumn("dbo.TransportPrices", "End");
            DropColumn("dbo.TransportPrices", "Start");
        }
    }
}
