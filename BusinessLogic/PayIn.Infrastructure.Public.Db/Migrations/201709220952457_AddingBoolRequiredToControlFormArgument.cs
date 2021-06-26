namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingBoolRequiredToControlFormArgument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlFormArguments", "Required", c => c.Boolean(defaultValue:false));
			Sql("UPDATE dbo.ControlFormArguments SET Required = 0");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlFormArguments", "Required");
        }
    }
}
