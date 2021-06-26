namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_updateTrackFrecuencyType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlItems", "TrackFrecuency", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlItems", "TrackFrecuency__Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlItems", "TrackFrecuency__Value", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlItems", "TrackFrecuency");
        }
    }
}
