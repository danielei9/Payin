namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_3_3_0_DeleteVBLBFromGreyList : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GreyLists", "VBLB");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GreyLists", "VBLB", c => c.Boolean(nullable: false));
        }
    }
}
