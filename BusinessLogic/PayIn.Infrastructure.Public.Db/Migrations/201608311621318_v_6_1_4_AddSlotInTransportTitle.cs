namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_4_AddSlotInTransportTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "Slot", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "Slot");
        }
    }
}
