namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddNullableFKRechargePayment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Payments", new[] { "RechargeId" });
            AlterColumn("dbo.Payments", "RechargeId", c => c.Int());
            CreateIndex("dbo.Payments", "RechargeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payments", new[] { "RechargeId" });
            //AlterColumn("dbo.Payments", "RechargeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Payments", "RechargeId");
        }
    }
}
