namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperationWalletRelation : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TransportOperationTitles", new[] { "TitleId" });
            AddColumn("dbo.TransportOperations", "SystemCardId", c => c.Int());
            AddColumn("dbo.TransportOperationTitles", "Unities", c => c.String(nullable: false));
            AddColumn("dbo.TransportOperationTitles", "Slot", c => c.Int());
            AddColumn("dbo.TransportOperationTitles", "PurseId", c => c.Int());
            AlterColumn("dbo.TransportOperationTitles", "TitleId", c => c.Int());
            CreateIndex("dbo.TransportOperations", "SystemCardId");
            CreateIndex("dbo.TransportOperationTitles", "TitleId");
            CreateIndex("dbo.TransportOperationTitles", "PurseId");
            AddForeignKey("dbo.TransportOperationTitles", "PurseId", "dbo.Purses", "Id");
            AddForeignKey("dbo.TransportOperations", "SystemCardId", "dbo.SystemCards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransportOperations", "SystemCardId", "dbo.SystemCards");
            DropForeignKey("dbo.TransportOperationTitles", "PurseId", "dbo.Purses");
            DropIndex("dbo.TransportOperationTitles", new[] { "PurseId" });
            DropIndex("dbo.TransportOperationTitles", new[] { "TitleId" });
            DropIndex("dbo.TransportOperations", new[] { "SystemCardId" });
            AlterColumn("dbo.TransportOperationTitles", "TitleId", c => c.Int(nullable: false));
            DropColumn("dbo.TransportOperationTitles", "PurseId");
            DropColumn("dbo.TransportOperationTitles", "Slot");
            DropColumn("dbo.TransportOperationTitles", "Unities");
            DropColumn("dbo.TransportOperations", "SystemCardId");
            CreateIndex("dbo.TransportOperationTitles", "TitleId");
        }
    }
}
