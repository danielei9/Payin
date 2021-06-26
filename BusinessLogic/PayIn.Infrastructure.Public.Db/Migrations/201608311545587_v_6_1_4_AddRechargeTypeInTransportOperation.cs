namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_4_AddRechargeTypeInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "RechargeType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "RechargeType");
        }
    }
}
