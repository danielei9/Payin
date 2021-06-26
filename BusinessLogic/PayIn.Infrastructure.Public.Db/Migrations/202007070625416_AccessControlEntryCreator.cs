namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessControlEntryCreator : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccessControlEntries", "CreatorLogin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccessControlEntries", "CreatorLogin");
        }
    }
}
