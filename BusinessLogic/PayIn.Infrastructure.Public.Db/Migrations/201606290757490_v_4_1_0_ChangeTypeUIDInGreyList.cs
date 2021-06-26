namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_ChangeTypeUIDInGreyList : DbMigration
    {
        public override void Up()
        {
			AlterColumn("dbo.greyLists", "Uid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
			AlterColumn("dbo.greyLists", "Uid", c => c.String());

        }
    }
}
