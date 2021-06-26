namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FieldIsRequiredAddToFormArgument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlFormArguments", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlFormArguments", "IsRequired");
        }
    }
}
