namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentConcessionCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "CityId", c => c.Int());
            CreateIndex("dbo.PaymentConcessions", "CityId");
            AddForeignKey("dbo.PaymentConcessions", "CityId", "dbo.ServiceCities", "Id");
        }
        public override void Down()
        {
            DropForeignKey("dbo.PaymentConcessions", "CityId", "dbo.ServiceCities");
            DropIndex("dbo.PaymentConcessions", new[] { "CityId" });
            DropColumn("dbo.PaymentConcessions", "CityId");
        }
    }
}
