namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiceCitytoServiceConcessionwithnewAttributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceConcessions", "VisibleCommerce", c => c.Boolean(nullable: false, defaultValue:false));
            AddColumn("dbo.ServiceCities", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.ServiceCities", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.ServiceCities", "ConcessionId", c => c.Int());
            CreateIndex("dbo.ServiceCities", "ConcessionId");
            AddForeignKey("dbo.ServiceCities", "ConcessionId", "dbo.ServiceConcessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceCities", "ConcessionId", "dbo.ServiceConcessions");
            DropIndex("dbo.ServiceCities", new[] { "ConcessionId" });
            DropColumn("dbo.ServiceCities", "ConcessionId");
            DropColumn("dbo.ServiceCities", "Longitude");
            DropColumn("dbo.ServiceCities", "Latitude");
            DropColumn("dbo.ServiceConcessions", "VisibleCommerce");
        }
    }
}
