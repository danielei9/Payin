namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SystemCardPurseRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purses", "SystemCardId", c => c.Int());
            CreateIndex("dbo.Purses", "SystemCardId");
            AddForeignKey("dbo.Purses", "SystemCardId", "dbo.SystemCards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Purses", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.Purses", new[] { "SystemCardId" });
            DropColumn("dbo.Purses", "SystemCardId");
        }
    }
}
