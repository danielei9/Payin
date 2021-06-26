namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBlackList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlackLists", "VBLN", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlackLists", "UDLN", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlackLists", "UDLN");
            DropColumn("dbo.BlackLists", "VBLN");
        }
    }
}
