namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentsToSystemCard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                        SystemCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCards", t => t.SystemCardId)
                .Index(t => t.SystemCardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceDocuments", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.ServiceDocuments", new[] { "SystemCardId" });
            DropTable("dbo.ServiceDocuments");
        }
    }
}
