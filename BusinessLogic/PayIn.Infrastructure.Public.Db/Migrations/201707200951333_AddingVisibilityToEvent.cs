namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingVisibilityToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Visibility", c => c.Int(nullable: false, defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Visibility");
        }
    }
}
