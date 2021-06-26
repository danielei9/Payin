namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_3_0_TicketTemplateAndTicketModification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegEx = c.String(nullable: false),
                        PreviousTextPosition = c.String(nullable: false),
                        LaterTextPosition = c.String(nullable: false),
                        DateFormat = c.String(nullable: false),
                        ReferencePosition = c.Int(),
                        TitlePosition = c.Int(),
                        DatePosition = c.Int(),
                        AmountPosition = c.Int(nullable: false),
                        WorkerPosition = c.Int(),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            AddColumn("dbo.Tickets", "TextUrl", c => c.String(nullable: false, defaultValue: ""));
			AddColumn("dbo.Tickets", "PdfUrl", c => c.String(nullable: false, defaultValue: ""));
            AddColumn("dbo.Tickets", "TemplateId", c => c.Int());
            CreateIndex("dbo.Tickets", "TemplateId");
            AddForeignKey("dbo.Tickets", "TemplateId", "dbo.TicketTemplates", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TemplateId", "dbo.TicketTemplates");
            DropForeignKey("dbo.TicketTemplates", "ConcessionId", "dbo.PaymentConcessions");
            DropIndex("dbo.TicketTemplates", new[] { "ConcessionId" });
            DropIndex("dbo.Tickets", new[] { "TemplateId" });
            DropColumn("dbo.Tickets", "TemplateId");
            DropColumn("dbo.Tickets", "PdfUrl");
            DropColumn("dbo.Tickets", "TextUrl");
            DropTable("dbo.TicketTemplates");
        }
    }
}
