namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceCardBatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCardBatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        State = c.Int(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                        SystemCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCards", t => t.SystemCardId)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId)
                .Index(t => t.SystemCardId);
            
            AddColumn("dbo.ServiceCards", "ServiceCardBatchId", c => c.Int());
            CreateIndex("dbo.ServiceCards", "ServiceCardBatchId");
            AddForeignKey("dbo.ServiceCards", "ServiceCardBatchId", "dbo.ServiceCardBatches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCardBatches", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceCards", "ServiceCardBatchId", "dbo.ServiceCardBatches");
            DropForeignKey("dbo.ServiceCardBatches", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.ServiceCards", new[] { "ServiceCardBatchId" });
            DropIndex("dbo.ServiceCardBatches", new[] { "SystemCardId" });
            DropIndex("dbo.ServiceCardBatches", new[] { "ConcessionId" });
            DropColumn("dbo.ServiceCards", "ServiceCardBatchId");
            DropTable("dbo.ServiceCardBatches");
        }
    }
}
