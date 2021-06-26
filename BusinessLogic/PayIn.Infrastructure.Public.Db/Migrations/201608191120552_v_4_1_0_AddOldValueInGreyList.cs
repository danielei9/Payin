namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddOldValueInGreyList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreyLists", "OldValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreyLists", "OldValue");
        }
    }
}
