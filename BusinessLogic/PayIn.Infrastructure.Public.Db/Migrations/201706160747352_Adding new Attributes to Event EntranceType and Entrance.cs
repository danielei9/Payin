namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingnewAttributestoEventEntranceTypeandEntrance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entrances", "SendingCount", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.EntranceTypes", "MaxSendingCount", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.EntranceTypes", "ShortDescription", c => c.String(nullable: false));
            AddColumn("dbo.EntranceTypes", "Conditions", c => c.String(nullable: false));
            AddColumn("dbo.Events", "ShortDescription", c => c.String(nullable: false));
            AddColumn("dbo.Events", "Conditions", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Conditions");
            DropColumn("dbo.Events", "ShortDescription");
            DropColumn("dbo.EntranceTypes", "Conditions");
            DropColumn("dbo.EntranceTypes", "ShortDescription");
            DropColumn("dbo.EntranceTypes", "MaxSendingCount");
            DropColumn("dbo.Entrances", "SendingCount");
        }
    }
}
