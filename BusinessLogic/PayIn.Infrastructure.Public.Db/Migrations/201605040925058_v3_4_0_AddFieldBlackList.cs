namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_4_0_AddFieldBlackList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreyLists", "OperationNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreyLists", "OperationNumber");
        }
    }
}
