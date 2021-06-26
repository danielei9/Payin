namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_ConcessionTemplateRelationSwap : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketTemplates", "ConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.TicketTemplates", new[] { "ConcessionId" });
            AddColumn("dbo.PaymentConcessions", "TicketTemplateId", c => c.Int());
            CreateIndex("dbo.PaymentConcessions", "TicketTemplateId");
            AddForeignKey("dbo.PaymentConcessions", "TicketTemplateId", "dbo.TicketTemplates", "Id");
            DropColumn("dbo.TicketTemplates", "ConcessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TicketTemplates", "ConcessionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PaymentConcessions", "TicketTemplateId", "dbo.TicketTemplates");
            DropIndex("dbo.PaymentConcessions", new[] { "TicketTemplateId" });
            DropColumn("dbo.PaymentConcessions", "TicketTemplateId");
            CreateIndex("dbo.TicketTemplates", "ConcessionId");
            AddForeignKey("dbo.TicketTemplates", "ConcessionId", "dbo.PaymentConcessions", "Id");
        }
    }
}
