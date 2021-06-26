namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_Ticket : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Tickets", new[] { "PaymentWorkerId" });
            AlterColumn("dbo.Tickets", "PaymentWorkerId", c => c.Int());
            CreateIndex("dbo.Tickets", "PaymentWorkerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tickets", new[] { "PaymentWorkerId" });
            AlterColumn("dbo.Tickets", "PaymentWorkerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "PaymentWorkerId");
        }
    }
}
