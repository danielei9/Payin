namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddPaymentConcessionInTransportConcession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportConcessions", "PaymentConcessionId", c => c.Int());
            CreateIndex("dbo.TransportConcessions", "PaymentConcessionId");
            AddForeignKey("dbo.TransportConcessions", "PaymentConcessionId", "dbo.PaymentConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportConcessions", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.TransportConcessions", new[] { "PaymentConcessionId" });
            DropColumn("dbo.TransportConcessions", "PaymentConcessionId");
        }
    }
}
