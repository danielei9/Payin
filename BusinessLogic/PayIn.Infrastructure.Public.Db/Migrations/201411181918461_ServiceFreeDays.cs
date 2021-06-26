namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceFreeDays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceFreeDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        From = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceFreeDays", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ServiceFreeDays", new[] { "ConcessionId" });
            DropTable("dbo.ServiceFreeDays");
        }
    }
}
