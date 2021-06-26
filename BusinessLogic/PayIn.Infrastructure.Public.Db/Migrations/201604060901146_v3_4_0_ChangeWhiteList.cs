namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_4_0_ChangeWhiteList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WhiteLists", "Uid", c => c.Long(nullable: false));
            AddColumn("dbo.WhiteLists", "OperationType", c => c.Int(nullable: false));
            AddColumn("dbo.WhiteLists", "TitleType", c => c.Int(nullable: false));
            AddColumn("dbo.WhiteLists", "InclusionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.WhiteLists", "ExclusionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.WhiteLists", "CardNumber");
            DropColumn("dbo.WhiteLists", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WhiteLists", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.WhiteLists", "CardNumber", c => c.Int(nullable: false));
            DropColumn("dbo.WhiteLists", "ExclusionDate");
            DropColumn("dbo.WhiteLists", "InclusionDate");
            DropColumn("dbo.WhiteLists", "TitleType");
            DropColumn("dbo.WhiteLists", "OperationType");
            DropColumn("dbo.WhiteLists", "Uid");
        }
    }
}
