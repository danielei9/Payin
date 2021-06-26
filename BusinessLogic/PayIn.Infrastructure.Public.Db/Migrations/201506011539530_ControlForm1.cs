namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlForm1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Observations = c.String(),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            CreateTable(
                "dbo.ControlFormArguments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Observations = c.String(),
                        Type = c.Int(nullable: false),
                        Target = c.Int(nullable: false),
                        FormId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlForms", t => t.FormId)
                .Index(t => t.FormId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlForms", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ControlFormArguments", "FormId", "dbo.ControlForms");
            DropIndex("dbo.ControlFormArguments", new[] { "FormId" });
            DropIndex("dbo.ControlForms", new[] { "ConcessionId" });
            DropTable("dbo.ControlFormArguments");
            DropTable("dbo.ControlForms");
        }
    }
}
