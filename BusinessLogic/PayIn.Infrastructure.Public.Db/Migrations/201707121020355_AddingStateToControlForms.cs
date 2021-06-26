namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStateToControlForms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlForms", "State", c => c.Int(nullable: false, defaultValue:1));
            AddColumn("dbo.ControlFormArguments", "State", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.ControlFormOptions", "State", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlFormOptions", "State");
            DropColumn("dbo.ControlFormArguments", "State");
            DropColumn("dbo.ControlForms", "State");
        }
    }
}
