namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMinAndMaxOptionsToFormArguments : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.ControlFormArguments", "MinOptions", c => c.Int());
			Sql("UPDATE dbo.ControlFormArguments SET MinOptions = IsRequired");
			AlterColumn("dbo.ControlFormArguments", "MinOptions", c => c.Int(nullable: false));
            AddColumn("dbo.ControlFormArguments", "MaxOptions", c => c.Int());
            DropColumn("dbo.ControlFormArguments", "IsRequired");
        }
        
        public override void Down()
        {
			AddColumn("dbo.ControlFormArguments", "IsRequired", c => c.Boolean());
			Sql("UPDATE dbo.ControlFormArguments SET IsRequired = MinOptions");
			AlterColumn("dbo.ControlFormArguments", "IsRequired", c => c.Boolean(nullable: false));
            DropColumn("dbo.ControlFormArguments", "MaxOptions");
            DropColumn("dbo.ControlFormArguments", "MinOptions");
        }
    }
}
