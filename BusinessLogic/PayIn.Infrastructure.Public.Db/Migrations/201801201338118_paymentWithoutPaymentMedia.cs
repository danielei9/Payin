namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentWithoutPaymentMedia : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Payments", new[] { "PaymentMediaId" });
            AlterColumn("dbo.Payments", "PaymentMediaId", c => c.Int());
            CreateIndex("dbo.Payments", "PaymentMediaId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Payments", new[] { "PaymentMediaId" });
            AlterColumn("dbo.Payments", "PaymentMediaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Payments", "PaymentMediaId");
        }
    }
}
