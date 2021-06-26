namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVBLBGreyList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreyLists", "VBLB", c => c.Boolean(nullable: false));
            AlterColumn("dbo.GreyLists", "SerialNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GreyLists", "SerialNumber", c => c.Long(nullable: false));
            DropColumn("dbo.GreyLists", "VBLB");
        }
    }
}
