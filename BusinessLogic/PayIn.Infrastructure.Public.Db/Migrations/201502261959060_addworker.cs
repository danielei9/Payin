namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addworker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceWorkers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        HasAccepted = c.Boolean(nullable: false),
                        SupplierId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSuppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceWorkers", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.ServiceWorkers", new[] { "SupplierId" });
            DropTable("dbo.ServiceWorkers");
        }
    }
}
