namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddPromoExecutionIdInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "PromoExecutionId", c => c.Int());
            AddColumn("dbo.PromoExecutions", "TransportOperationId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PromoExecutions", "TransportOperationId");
            DropColumn("dbo.TransportOperations", "PromoExecutionId");
        }
    }
}
