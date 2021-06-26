namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOrderToControlFormArgument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlFormArguments", "Order", c => c.Int(nullable: false, defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlFormArguments", "Order");
        }
    }
}
