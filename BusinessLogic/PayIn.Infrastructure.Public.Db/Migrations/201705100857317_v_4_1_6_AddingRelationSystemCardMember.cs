namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_AddingRelationSystemCardMember : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceConcessionSystemCards", "ServiceConcession_Id", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceConcessionSystemCards", "SystemCard_Id", "dbo.SystemCards");
            DropIndex("dbo.ServiceConcessionSystemCards", new[] { "ServiceConcession_Id" });
            DropIndex("dbo.ServiceConcessionSystemCards", new[] { "SystemCard_Id" });
            CreateTable(
                "dbo.SystemCardMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CanEmit = c.Boolean(nullable: false),
                        SystemCardId = c.Int(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCards", t => t.SystemCardId)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.SystemCardId)
                .Index(t => t.ConcessionId);
            
            DropTable("dbo.ServiceConcessionSystemCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceConcessionSystemCards",
                c => new
                    {
                        ServiceConcession_Id = c.Int(nullable: false),
                        SystemCard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceConcession_Id, t.SystemCard_Id });
            
            DropForeignKey("dbo.SystemCardMembers", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.SystemCardMembers", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.SystemCardMembers", new[] { "ConcessionId" });
            DropIndex("dbo.SystemCardMembers", new[] { "SystemCardId" });
            DropTable("dbo.SystemCardMembers");
            CreateIndex("dbo.ServiceConcessionSystemCards", "SystemCard_Id");
            CreateIndex("dbo.ServiceConcessionSystemCards", "ServiceConcession_Id");
            AddForeignKey("dbo.ServiceConcessionSystemCards", "SystemCard_Id", "dbo.SystemCards", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceConcessionSystemCards", "ServiceConcession_Id", "dbo.ServiceConcessions", "Id", cascadeDelete: true);
        }
    }
}
