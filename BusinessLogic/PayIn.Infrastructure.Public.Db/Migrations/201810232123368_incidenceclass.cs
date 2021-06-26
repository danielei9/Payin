namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incidenceclass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceIncidences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        SubCategory = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ServiceNotifications", "IncidenceId", c => c.Int());
            CreateIndex("dbo.ServiceNotifications", "IncidenceId");
            AddForeignKey("dbo.ServiceNotifications", "IncidenceId", "dbo.ServiceIncidences", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceNotifications", "IncidenceId", "dbo.ServiceIncidences");
            DropIndex("dbo.ServiceNotifications", new[] { "IncidenceId" });
            DropColumn("dbo.ServiceNotifications", "IncidenceId");
            DropTable("dbo.ServiceIncidences");
        }
    }
}
