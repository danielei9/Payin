namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceOptionalEntranceType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Entrances", new[] { "EntranceTypeId" });
            AddColumn("dbo.Entrances", "EventId", c => c.Int(nullable: false));

			Sql(
				"UPDATE dbo.Entrances " +
				"SET EventId = ET.EventId " +
				"FROM " +
					"dbo.Entrances E INNER JOIN " +
					"dbo.EntranceTypes ET ON E.EntranceTypeId = ET.Id"
			);

            AlterColumn("dbo.Entrances", "EntranceTypeId", c => c.Int());
            CreateIndex("dbo.Entrances", "EntranceTypeId");
            CreateIndex("dbo.Entrances", "EventId");
            AddForeignKey("dbo.Entrances", "EventId", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entrances", "EventId", "dbo.Events");
            DropIndex("dbo.Entrances", new[] { "EventId" });
            DropIndex("dbo.Entrances", new[] { "EntranceTypeId" });
            AlterColumn("dbo.Entrances", "EntranceTypeId", c => c.Int(nullable: false));
            DropColumn("dbo.Entrances", "EventId");
            CreateIndex("dbo.Entrances", "EntranceTypeId");
        }
    }
}
