namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_ChangeRelationEventToPaymentConcession : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.Events", new[] { "ConcessionId" });
            AddColumn("dbo.Events", "PaymentConcessionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "PaymentConcessionId");
            AddForeignKey("dbo.Events", "PaymentConcessionId", "dbo.PaymentConcessions", "Id");
            DropColumn("dbo.Events", "ConcessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "ConcessionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Events", "PaymentConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.Events", new[] { "PaymentConcessionId" });
            DropColumn("dbo.Events", "PaymentConcessionId");
            CreateIndex("dbo.Events", "ConcessionId");
            AddForeignKey("dbo.Events", "ConcessionId", "dbo.ServiceConcessions", "Id");
        }
    }
}
