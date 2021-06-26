namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_3_ChangeRaechargePaymentFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "RechargeId", "dbo.Recharges");
            DropIndex("dbo.Payments", new[] { "RechargeId" });
            AddColumn("dbo.Recharges", "PaymentId", c => c.Int());
			Sql(
				"UPDATE dbo.Recharges " +
				"SET PaymentId = P.Id " +
				"FROM dbo.Recharges R " +
					"INNER JOIN dbo.Payments P " +
					"ON P.RechargeId = R.Id"
				);
			CreateIndex("dbo.Recharges", "PaymentId");
            AddForeignKey("dbo.Recharges", "PaymentId", "dbo.Payments", "Id");
            DropColumn("dbo.Payments", "RechargeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "RechargeId", c => c.Int());
            DropForeignKey("dbo.Recharges", "PaymentId", "dbo.Payments");
            DropIndex("dbo.Recharges", new[] { "PaymentId" });
			Sql(
				"UPDATE dbo.Payments " +
				"SET RechargeId = R.Id " +
				"FROM dbo.Payments P " +
					"INNER JOIN dbo.Recharges R " +
					"ON R.PaymentId = P.Id"
			);
			DropColumn("dbo.Recharges", "PaymentId");
            CreateIndex("dbo.Payments", "RechargeId");
            AddForeignKey("dbo.Payments", "RechargeId", "dbo.Recharges", "Id");
        }
    }
}
