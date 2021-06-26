namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	using Xp.Common;

	public partial class TransportOperationTitleChanges : DbMigration
    {
        public override void Up()
        {
			Sql(
				"Delete From dbo.TransportOperationTitles Where TransportOperationId = null or TransportTitleId = null"
				);
            DropIndex("dbo.TransportOperationTitles", new[] { "TransportOperationId" });
            DropIndex("dbo.TransportOperationTitles", new[] { "TransportTitleId" });
            RenameColumn(table: "dbo.TransportOperationTitles", name: "TransportOperationId", newName: "OperationId");
            RenameColumn(table: "dbo.TransportOperationTitles", name: "TransportTitleId", newName: "TitleId");
            RenameColumn(table: "dbo.TransportOperationTitles", name :"Date", newName: "Caducity");
            AlterColumn("dbo.TransportOperationTitles", "Quantity", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TransportOperationTitles", "OperationId", c => c.Int(nullable: false));
            AlterColumn("dbo.TransportOperationTitles", "TitleId", c => c.Int(nullable: false));
			AlterColumn("dbo.TransportOperationTitles", "Caducity", c => c.DateTime(nullable: true));
			CreateIndex("dbo.TransportOperationTitles", "OperationId");
            CreateIndex("dbo.TransportOperationTitles", "TitleId");
        }
        
        public override void Down()
        {
			Sql(
				"Update dbo.TransportOperationTitles SET Caducity = '20121212' Where Caducity is null"
				);
            DropIndex("dbo.TransportOperationTitles", new[] { "TitleId" });
            DropIndex("dbo.TransportOperationTitles", new[] { "OperationId" });
            AlterColumn("dbo.TransportOperationTitles", "TitleId", c => c.Int());
            AlterColumn("dbo.TransportOperationTitles", "OperationId", c => c.Int());
            AlterColumn("dbo.TransportOperationTitles", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
			AlterColumn("dbo.TransportOperationTitles", "Caducity", c => c.DateTime(nullable: false));
			RenameColumn(table: "dbo.TransportOperationTitles", name: "Caducity", newName: "Date");
			RenameColumn(table: "dbo.TransportOperationTitles", name: "TitleId", newName: "TransportTitleId");
            RenameColumn(table: "dbo.TransportOperationTitles", name: "OperationId", newName: "TransportOperationId");
            CreateIndex("dbo.TransportOperationTitles", "TransportTitleId");
            CreateIndex("dbo.TransportOperationTitles", "TransportOperationId");
        }
    }
}
