namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LN_LG_LB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WhiteLists", "Source", c => c.Int(nullable: false));
            AddColumn("dbo.WhiteLists", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.WhiteLists", "ExclusionDate", c => c.DateTime());
            CreateIndex("dbo.WhiteLists", "Uid", name: "UidIndex");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WhiteLists", "UidIndex");
            AlterColumn("dbo.WhiteLists", "ExclusionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WhiteLists", "State");
            DropColumn("dbo.WhiteLists", "Source");
        }
    }
}
