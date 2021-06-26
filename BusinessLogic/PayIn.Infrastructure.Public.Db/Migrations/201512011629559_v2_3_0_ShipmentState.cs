namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_ShipmentState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "State");
        }
    }
}
