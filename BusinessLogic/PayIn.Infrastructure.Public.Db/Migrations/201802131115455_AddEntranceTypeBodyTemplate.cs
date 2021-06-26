namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEntranceTypeBodyTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "BodyTemplate", c => c.String(nullable: false, defaultValue: ""));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntranceTypes", "BodyTemplate");
        }
    }
}
