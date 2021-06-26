namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingVisibilityToEntranceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "Visibility", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntranceTypes", "Visibility");
        }
    }
}
