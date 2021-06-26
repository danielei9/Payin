namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_3_AddRelationRechargePayment : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Payments", "RechargeId", c => c.Int(nullable: false));
			AddColumn("dbo.Payments", "RechargeId", c => c.Int());
			CreateIndex("dbo.Payments", "RechargeId");
            AddForeignKey("dbo.Payments", "RechargeId", "dbo.Recharges", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "RechargeId", "dbo.Recharges");
            DropIndex("dbo.Payments", new[] { "RechargeId" });
            DropColumn("dbo.Payments", "RechargeId");
        }
    }
}
