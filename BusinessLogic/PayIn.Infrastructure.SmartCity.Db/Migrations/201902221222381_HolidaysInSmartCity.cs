namespace PayIn.Infrastructure.SmartCity.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HolidaysInSmartCity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SmartCity.Holidays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("SmartCity.Concessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("SmartCity.Holidays", "ConcessionId", "SmartCity.Concessions");
            DropIndex("SmartCity.Holidays", new[] { "ConcessionId" });
            DropTable("SmartCity.Holidays");
        }
    }
}
