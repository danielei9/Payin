namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chageObservationToReadValue : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("SmartCity.Observations", "SensorId", "SmartCity.Sensors");
            DropForeignKey("SmartCity.Sensors", "ProviderId", "SmartCity.Providers");
            DropIndex("SmartCity.Observations", new[] { "SensorId" });
            DropIndex("SmartCity.Sensors", new[] { "ProviderId" });

			CreateTable(
				"SmartCity.DataSets",
				c => new
				{
					Id = c.Int(nullable: false, identity: true),
					Timestamp = c.DateTime(nullable: false),
					ComponentId = c.Int(nullable: false),
				})
				.PrimaryKey(t => t.Id)
				.ForeignKey("SmartCity.Components", t => t.ComponentId)
				.Index(t => t.ComponentId);
			Sql(
				"INSERT SmartCity.DataSets (Timestamp, ComponentId) " +
				"SELECT DISTINCT O.Timestamp, S.ComponentId " +
				"FROM " +
					"SmartCity.Observations O INNER JOIN " +
					"SmartCity.Sensors S ON O.SensorId = S.Id "
			);

			CreateTable(
                "SmartCity.Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataSetId = c.Int(nullable: false),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Sensors", t => t.SensorId)
                .ForeignKey("SmartCity.DataSets", t => t.DataSetId)
                .Index(t => t.DataSetId)
                .Index(t => t.SensorId);
			Sql(
				"INSERT SmartCity.Data (DataSetId, SensorId, Value) " +
				"SELECT DISTINCT DS.Id, O.SensorId, O.Value " +
				"FROM " +
					"SmartCity.Observations O INNER JOIN " +
					"SmartCity.Sensors S ON O.SensorId = S.Id INNER JOIN " +
					"SmartCity.DataSets DS ON DS.Timestamp=O.Timestamp AND DS.ComponentId=S.ComponentId"
			);

			AddColumn("SmartCity.Components", "ProviderId", c => c.Int(nullable: true));
			Sql(
				"UPDATE SmartCity.Components " +
				"SET ProviderId = S.ProviderId " +
				"FROM " +
					"SmartCity.Components C INNER JOIN " +
					"SmartCity.Sensors S ON S.ComponentId=C.Id"
			);
			AlterColumn("SmartCity.Components", "ProviderId", c => c.Int(nullable: false));
			CreateIndex("SmartCity.Components", "ProviderId");
            AddForeignKey("SmartCity.Components", "ProviderId", "SmartCity.Providers", "Id");

            DropColumn("SmartCity.Sensors", "ProviderId");
            DropTable("SmartCity.Observations");
        }
        
        public override void Down()
        {
            CreateTable(
                "SmartCity.Observations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Latitude = c.Decimal(precision: 9, scale: 6),
                        Longitude = c.Decimal(precision: 9, scale: 6),
                        SensorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("SmartCity.Sensors", "ProviderId", c => c.Int(nullable: false));
            DropForeignKey("SmartCity.Data", "DataSetId", "SmartCity.DataSets");
            DropForeignKey("SmartCity.Data", "SensorId", "SmartCity.Sensors");
            DropForeignKey("SmartCity.Components", "ProviderId", "SmartCity.Providers");
            DropForeignKey("SmartCity.DataSets", "ComponentId", "SmartCity.Components");
            DropIndex("SmartCity.Components", new[] { "ProviderId" });
            DropIndex("SmartCity.DataSets", new[] { "ComponentId" });
            DropIndex("SmartCity.Data", new[] { "SensorId" });
            DropIndex("SmartCity.Data", new[] { "DataSetId" });
            DropColumn("SmartCity.Components", "ProviderId");
            DropTable("SmartCity.DataSets");
            DropTable("SmartCity.Data");
            CreateIndex("SmartCity.Sensors", "ProviderId");
            CreateIndex("SmartCity.Observations", "SensorId");
            AddForeignKey("SmartCity.Sensors", "ProviderId", "SmartCity.Providers", "Id");
            AddForeignKey("SmartCity.Observations", "SensorId", "SmartCity.Sensors", "Id");
        }
    }
}
