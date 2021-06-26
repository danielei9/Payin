namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealCheckPointWithNullTag : DbMigration
    {
		public override void Up()
		{
			DropIndex("dbo.ServiceCheckPoints", new[] { "TagId" });
			AlterColumn("dbo.ServiceCheckPoints", "TagId", c => c.Int(nullable: true));
			CreateIndex("dbo.ServiceCheckPoints", "TagId");
		}
		
		public override void Down()
		{
			DropIndex("dbo.ServiceCheckPoints", new[] { "TagId" });
			AlterColumn("dbo.ServiceCheckPoints", "TagId", c => c.Int(nullable: false));
			CreateIndex("dbo.ServiceCheckPoints", "TagId");
		}
    }
}
