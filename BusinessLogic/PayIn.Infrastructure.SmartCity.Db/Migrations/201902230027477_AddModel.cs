namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.ModelSchedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        WeekDay = c.Int(nullable: false),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        DeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Devices", t => t.DeviceId)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "SmartCity.ModelTimeTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Since = c.DateTime(nullable: false),
                        Until = c.DateTime(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.ModelSchedules", t => t.ScheduleId)
                .Index(t => t.ScheduleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.ModelSchedules", "DeviceId", "SmartCity.Devices");
            DropForeignKey("SmartCity.ModelTimeTables", "ScheduleId", "SmartCity.ModelSchedules");
            DropIndex("SmartCity.ModelTimeTables", new[] { "ScheduleId" });
            DropIndex("SmartCity.ModelSchedules", new[] { "DeviceId" });
            DropTable("SmartCity.ModelTimeTables");
            DropTable("SmartCity.ModelSchedules");
        }
    }
}
