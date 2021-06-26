namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_addTrackFrecuencyControlItem : DbMigration
    {
        public override void Up()
        {
			AddColumn("dbo.ControlItems", "TrackFrecuency__Value", c => c.DateTime(nullable: true));
			Sql(
				"UPDATE dbo.ControlItems " +
				"SET TrackFrecuency__Value = '1900-01-01 00:00:10' "
				);


			AlterColumn("dbo.ControlItems", "TrackFrecuency__Value", c => c.DateTime(nullable: false));
		}
        
        public override void Down()
        {
			DropColumn("dbo.ControlItems", "TrackFrecuency__Value");
        }
    }
}
