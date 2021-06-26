namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateConcessionInSmartCityService : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.Concessions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Login = c.String(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("SmartCity.Devices", "ConcessionId", c => c.Int());
            CreateIndex("SmartCity.Devices", "ConcessionId");
            AddForeignKey("SmartCity.Devices", "ConcessionId", "SmartCity.Concessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.Devices", "ConcessionId", "SmartCity.Concessions");
            DropIndex("SmartCity.Devices", new[] { "ConcessionId" });
            DropColumn("SmartCity.Devices", "ConcessionId");
            DropTable("SmartCity.Concessions");
        }
    }
}
