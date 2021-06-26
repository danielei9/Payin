namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_4_0_GreyListAddSource : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreyLists", "Source", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreyLists", "Source");
        }
    }
}
