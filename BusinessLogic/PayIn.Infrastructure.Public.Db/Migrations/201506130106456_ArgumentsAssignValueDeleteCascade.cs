namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArgumentsAssignValueDeleteCascade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns");
            AddForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns");
            AddForeignKey("dbo.ControlFormValues", "AssignId", "dbo.ControlFormAssigns", "Id");
        }
    }
}
