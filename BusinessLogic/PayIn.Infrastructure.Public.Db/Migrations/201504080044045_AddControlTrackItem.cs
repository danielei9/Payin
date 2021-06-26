namespace PayIn.Infrastructure.Public.Db.Migrations
{
	using System;
	using System.Data.Entity.Migrations;

	public partial class AddControlTrackItem : DbMigration
	{
		public override void Up()
		{
			CreateTable(
					"dbo.ControlTrackItems",
					c => new
					{
						Id = c.Int(nullable: false, identity: true),
						CreatedAt = c.DateTime(nullable: false),
						Date = c.DateTime(nullable: false),
						Latitude = c.Decimal(nullable: false, precision: 9, scale: 6),
						Longitude = c.Decimal(nullable: false, precision: 9, scale: 6),
						Quality = c.Int(nullable: false),
						TrackId = c.Int(nullable: false),
					})
					.PrimaryKey(t => t.Id)
					.ForeignKey("dbo.ControlTracks", t => t.TrackId)
					.Index(t => t.TrackId);

			Sql("DELETE dbo.ControlTracks");
			//Sql(
			//	"INSERT dbo.ControlTrackItems (CreatedAt, [Date], Latitude, Longitude, Quality, TrackId) " +
			//	"SELECT " +
			//		"CT.CreatedAt, " +
			//		"CT.[Date], " +
			//		"CT.Latitude, " +
			//		"CT.Longitude, " +
			//		"CT.Quality, " +
			//		"( " +
			//			"SELECT MIN(CT2.Id) " +
			//			"FROM dbo.ControlTracks CT2 " +
			//			"WHERE ( " +
			//				"CT2.WorkerId = CT.WorkerId AND " +
			//				"CT2.ItemId = CT.ItemId AND " +
			//				"YEAR(CT2.[Date]) = YEAR(CT.[Date]) AND " +
			//				"MONTH(CT2.[Date]) = MONTH(CT.[Date]) AND " +
			//				"DAY(CT2.[Date]) = DAY(CT.[Date]) " +
			//			") " +
			//		") as TrackId " +
			//	"FROM dbo.ControlTracks CT "
			//);

			//Sql(
			//	"DELETE dbo.ControlTracks " +
			//	"FROM dbo.ControlTracks CT " +
			//	"WHERE Id > ( " +
			//		"SELECT MIN(CT2.Id) " +
			//			"FROM dbo.ControlTracks CT2 " +
			//			"WHERE ( " +
			//				"CT2.WorkerId = CT.WorkerId AND " +
			//				"CT2.ItemId = CT.ItemId AND " +
			//				"YEAR(CT2.[Date]) = YEAR(CT.[Date]) AND " +
			//				"MONTH(CT2.[Date]) = MONTH(CT.[Date]) AND " +
			//				"DAY(CT2.[Date]) = DAY(CT.[Date]) " +
			//			") " +
			//		") OR (NOT EXISTS( " +
			//			"SELECT * " +
			//			"FROM dbo.ControlPresences CP2 " +
			//			"WHERE ( " +
			//				"CP2.WorkerId = CT.WorkerId AND " +
			//				"CP2.ItemId = CT.ItemId AND " +
			//				"YEAR(CP2.[Date]) = YEAR(CT.[Date]) AND " +
			//				"MONTH(CP2.[Date]) = MONTH(CT.[Date]) AND " +
			//				"DAY(CP2.[Date]) = DAY(CT.[Date]) " +
			//			") " +
			//		")) "
			//);

			AddColumn("dbo.ControlTracks", "PresenceSinceId", c => c.Int(nullable: false));
			AddColumn("dbo.ControlTracks", "PresenceUntilId", c => c.Int());

			//Sql(
			//	"UPDATE dbo.ControlTracks " +
			//	"SET " + 
			//		"PresenceSinceId = ( " +
			//			"SELECT MIN(CP2.Id) " +
			//			"FROM dbo.ControlPresences CP2 " +
			//			"WHERE ( " +
			//				"CP2.WorkerId = CT.WorkerId AND " +
			//				"CP2.ItemId = CT.ItemId AND " +
			//				"YEAR(CP2.[Date]) = YEAR(CT.[Date]) AND " +
			//				"MONTH(CP2.[Date]) = MONTH(CT.[Date]) AND " +
			//				"DAY(CP2.[Date]) = DAY(CT.[Date]) " +
			//			") " +
			//		"), " +
			//		"PresenceUntilId = ( " +
			//			"SELECT MAX(CP2.Id) " +
			//			"FROM dbo.ControlPresences CP2 " +
			//			"WHERE ( " +
			//				"CP2.WorkerId = CT.WorkerId AND " +
			//				"CP2.ItemId = CT.ItemId AND " +
			//				"YEAR(CP2.[Date]) = YEAR(CT.[Date]) AND " +
			//				"MONTH(CP2.[Date]) = MONTH(CT.[Date]) AND " +
			//				"DAY(CP2.[Date]) = DAY(CT.[Date]) " +
			//			") " +
			//		") " +
			//	"FROM dbo.ControlTracks CT "
			//);

			CreateIndex("dbo.ControlTracks", "PresenceSinceId");
			CreateIndex("dbo.ControlTracks", "PresenceUntilId");
			AddForeignKey("dbo.ControlTracks", "PresenceSinceId", "dbo.ControlPresences", "Id");
			AddForeignKey("dbo.ControlTracks", "PresenceUntilId", "dbo.ControlPresences", "Id");
			DropColumn("dbo.ControlTracks", "Date");
			DropColumn("dbo.ControlTracks", "Latitude");
			DropColumn("dbo.ControlTracks", "Longitude");
			DropColumn("dbo.ControlTracks", "Quality");
		}

		public override void Down()
		{
			AddColumn("dbo.ControlTracks", "Quality", c => c.Int(nullable: false));
			AddColumn("dbo.ControlTracks", "Longitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
			AddColumn("dbo.ControlTracks", "Latitude", c => c.Decimal(nullable: false, precision: 9, scale: 6));
			AddColumn("dbo.ControlTracks", "Date", c => c.DateTime(nullable: false));
			DropForeignKey("dbo.ControlTracks", "PresenceUntilId", "dbo.ControlPresences");
			DropForeignKey("dbo.ControlTracks", "PresenceSinceId", "dbo.ControlPresences");
			DropForeignKey("dbo.ControlTrackItems", "TrackId", "dbo.ControlTracks");
			DropIndex("dbo.ControlTrackItems", new[] { "TrackId" });
			DropIndex("dbo.ControlTracks", new[] { "PresenceUntilId" });
			DropIndex("dbo.ControlTracks", new[] { "PresenceSinceId" });
			DropColumn("dbo.ControlTracks", "PresenceUntilId");
			DropColumn("dbo.ControlTracks", "PresenceSinceId");
			DropTable("dbo.ControlTrackItems");
		}
	}
}
