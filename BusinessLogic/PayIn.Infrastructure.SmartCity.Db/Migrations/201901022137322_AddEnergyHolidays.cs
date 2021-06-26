namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEnergyHolidays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.EnergyHolidays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("SmartCity.EnergyHolidays");
        }
    }
}
