namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Event_with_Code : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Code", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Code");
        }
    }
}
