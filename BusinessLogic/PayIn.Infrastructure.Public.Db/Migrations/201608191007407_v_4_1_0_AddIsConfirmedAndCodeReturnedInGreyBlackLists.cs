namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddIsConfirmedAndCodeReturnedInGreyBlackLists : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlackLists", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlackLists", "CodeReturned", c => c.String());
            AddColumn("dbo.GreyLists", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.GreyLists", "CodeReturned", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreyLists", "CodeReturned");
            DropColumn("dbo.GreyLists", "IsConfirmed");
            DropColumn("dbo.BlackLists", "CodeReturned");
            DropColumn("dbo.BlackLists", "IsConfirmed");
        }
    }
}
