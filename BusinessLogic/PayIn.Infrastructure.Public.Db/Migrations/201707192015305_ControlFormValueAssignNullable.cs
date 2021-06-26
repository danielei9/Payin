namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlFormValueAssignNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns");
            DropIndex("dbo.ControlFormValues", new[] { "AssignId" });
            AlterColumn("dbo.ControlFormValues", "AssignId", c => c.Int());
            CreateIndex("dbo.ControlFormValues", "AssignId");
            AddForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns");
            DropIndex("dbo.ControlFormValues", new[] { "AssignId" });
            AlterColumn("dbo.ControlFormValues", "AssignId", c => c.Int(nullable: false));
            CreateIndex("dbo.ControlFormValues", "AssignId");
            AddForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns", "Id", cascadeDelete: true);
        }
    }
}
