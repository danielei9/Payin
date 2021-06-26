namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceEntity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("SmartCity.Components", "ProviderId", "SmartCity.Providers");
            DropIndex("SmartCity.Components", new[] { "ProviderId" });
            CreateTable(
                "SmartCity.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ProviderName = c.String(nullable: false),
                        ProviderCode = c.String(nullable: false),
                        Model = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

			Sql(
				"INSERT INTO SmartCity.Devices (Name, ProviderName, ProviderCode, Model) " +
				"SELECT C.Name, P.Code, P.Code, C.Name " +
				"FROM " +
					"SmartCity.Components C INNER JOIN " +
					"SmartCity.Providers P ON C.ProviderId=P.Id "
			);
            
            AddColumn("SmartCity.Components", "ProviderName", c => c.String(nullable: false));
			Sql(
				"UPDATE SmartCity.Components " +
				"SET " +
					"ProviderName = P.Code " +
				"FROM " +
					"SmartCity.Components C INNER JOIN " +
					"SmartCity.Providers P ON C.ProviderId = P.Id "
			);

			AddColumn("SmartCity.Components", "DeviceId", c => c.Int(nullable: true));
			Sql(
				"UPDATE SmartCity.Components " +
				"SET " +
					"DeviceId = D.Id " +
				"FROM " +
					"SmartCity.Components C INNER JOIN " +
					"SmartCity.Providers P ON C.ProviderId = P.Id, " +
					"SmartCity.Devices D " +
				"WHERE " +
					"D.Name=C.Name AND " +
					"D.ProviderName=P.Code AND " +
					"D.ProviderCode=P.Code AND " +
					"D.Model=C.Name "
			);
			AlterColumn("SmartCity.Components", "DeviceId", c => c.Int(nullable: false));

			CreateIndex("SmartCity.Components", "DeviceId");
            AddForeignKey("SmartCity.Components", "DeviceId", "SmartCity.Devices", "Id");
            DropColumn("SmartCity.Components", "ProviderId");
            DropTable("SmartCity.Providers");
        }
        
        public override void Down()
        {
            CreateTable(
                "SmartCity.Providers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("SmartCity.Components", "ProviderId", c => c.Int(nullable: false));
            DropForeignKey("SmartCity.Components", "DeviceId", "SmartCity.Devices");
            DropIndex("SmartCity.Components", new[] { "DeviceId" });
            DropColumn("SmartCity.Components", "DeviceId");
            DropColumn("SmartCity.Components", "ProviderName");
            DropTable("SmartCity.Devices");
            CreateIndex("SmartCity.Components", "ProviderId");
            AddForeignKey("SmartCity.Components", "ProviderId", "SmartCity.Providers", "Id");
        }
    }
}
