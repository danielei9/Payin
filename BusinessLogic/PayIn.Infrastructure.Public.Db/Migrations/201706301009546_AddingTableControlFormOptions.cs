namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingTableControlFormOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControlFormOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Value = c.Int(),
                        ArgumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ControlFormArguments", t => t.ArgumentId)
                .Index(t => t.ArgumentId);
            
            CreateTable(
                "dbo.ControlFormOptionControlFormValues",
                c => new
                    {
                        ControlFormOption_Id = c.Int(nullable: false),
                        ControlFormValue_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ControlFormOption_Id, t.ControlFormValue_Id })
                .ForeignKey("dbo.ControlFormOptions", t => t.ControlFormOption_Id, cascadeDelete: true)
                .ForeignKey("dbo.ControlFormValues", t => t.ControlFormValue_Id, cascadeDelete: true)
                .Index(t => t.ControlFormOption_Id)
                .Index(t => t.ControlFormValue_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormOptions", "ArgumentId", "dbo.ControlFormArguments");
            DropForeignKey("dbo.ControlFormOptionControlFormValues", "ControlFormValue_Id", "dbo.ControlFormValues");
            DropForeignKey("dbo.ControlFormOptionControlFormValues", "ControlFormOption_Id", "dbo.ControlFormOptions");
            DropIndex("dbo.ControlFormOptionControlFormValues", new[] { "ControlFormValue_Id" });
            DropIndex("dbo.ControlFormOptionControlFormValues", new[] { "ControlFormOption_Id" });
            DropIndex("dbo.ControlFormOptions", new[] { "ArgumentId" });
            DropTable("dbo.ControlFormOptionControlFormValues");
            DropTable("dbo.ControlFormOptions");
        }
    }
}
