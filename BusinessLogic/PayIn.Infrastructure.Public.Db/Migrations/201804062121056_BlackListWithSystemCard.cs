namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlackListWithSystemCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlackLists", "SystemCardId", c => c.Int());
            AddColumn("dbo.GreyLists", "SystemCardId", c => c.Int());
            CreateIndex("dbo.BlackLists", "SystemCardId");
            CreateIndex("dbo.GreyLists", "SystemCardId");
            AddForeignKey("dbo.BlackLists", "SystemCardId", "dbo.SystemCards", "Id");
            AddForeignKey("dbo.GreyLists", "SystemCardId", "dbo.SystemCards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GreyLists", "SystemCardId", "dbo.SystemCards");
            DropForeignKey("dbo.BlackLists", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.GreyLists", new[] { "SystemCardId" });
            DropIndex("dbo.BlackLists", new[] { "SystemCardId" });
            DropColumn("dbo.GreyLists", "SystemCardId");
            DropColumn("dbo.BlackLists", "SystemCardId");
        }
    }
}
