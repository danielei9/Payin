namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_AddingTicketInEntitys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entrances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.Long(nullable: false),
                        State = c.Int(nullable: false),
                        VatNumber = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        EntranceTypeId = c.Int(nullable: false),
                        TicketLineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntranceTypes", t => t.EntranceTypeId)
                .ForeignKey("dbo.TicketLines", t => t.TicketLineId)
                .Index(t => t.EntranceTypeId)
                .Index(t => t.TicketLineId);
            
            CreateTable(
                "dbo.Checks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Login = c.String(nullable: false),
                        Observation = c.String(nullable: false),
                        EntranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entrances", t => t.EntranceId)
                .Index(t => t.EntranceId);
            
            CreateTable(
                "dbo.EntranceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PhotoUrl = c.String(nullable: false),
                        MaxEntrance = c.Int(),
                        SellStart = c.DateTime(nullable: false),
                        SellEnd = c.DateTime(nullable: false),
                        CheckInStart = c.DateTime(nullable: false),
                        CheckInEnd = c.DateTime(nullable: false),
                        ExtraPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Code = c.String(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Longitude = c.Decimal(precision: 18, scale: 2),
                        Latitude = c.Decimal(precision: 18, scale: 2),
                        Place = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        PhotoUrl = c.String(nullable: false),
                        Capacity = c.Int(),
                        MaxEntrance = c.Int(),
                        EventStart = c.DateTime(nullable: false),
                        EventEnd = c.DateTime(nullable: false),
                        ExtraPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CheckInStart = c.DateTime(nullable: false),
                        CheckInEnd = c.DateTime(nullable: false),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entrances", "TicketLineId", "dbo.TicketLines");
            DropForeignKey("dbo.EntranceTypes", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.Entrances", "EntranceTypeId", "dbo.EntranceTypes");
            DropForeignKey("dbo.Checks", "EntranceId", "dbo.Entrances");
            DropIndex("dbo.Events", new[] { "ConcessionId" });
            DropIndex("dbo.EntranceTypes", new[] { "EventId" });
            DropIndex("dbo.Checks", new[] { "EntranceId" });
            DropIndex("dbo.Entrances", new[] { "TicketLineId" });
            DropIndex("dbo.Entrances", new[] { "EntranceTypeId" });
            DropTable("dbo.Events");
            DropTable("dbo.EntranceTypes");
            DropTable("dbo.Checks");
            DropTable("dbo.Entrances");
        }
    }
}
