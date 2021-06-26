namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_4_CreateCityCard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Long(nullable: false),
                        State = c.Int(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        Login = c.String(nullable: false),
                        Photo = c.String(nullable: false),
                        VatNumber = c.String(),
                        ConcessionId = c.Int(nullable: false),
                        SystemCardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCards", t => t.SystemCardId)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId)
                .Index(t => t.SystemCardId);
            
            CreateTable(
                "dbo.SystemCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CardType = c.Int(nullable: false),
                        ConcessionOwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionOwnerId)
                .Index(t => t.ConcessionOwnerId);
            
            CreateTable(
                "dbo.ServiceUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        State = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Phone = c.Long(),
                        Address = c.String(nullable: false),
                        Email = c.String(),
                        BirthDay = c.DateTime(),
                        AssertDoc = c.String(),
                        ConcessionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceConcessions", t => t.ConcessionId)
                .Index(t => t.ConcessionId);
            
            CreateTable(
                "dbo.ServiceUserServiceCards",
                c => new
                    {
                        ServiceUser_Id = c.Int(nullable: false),
                        ServiceCard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceUser_Id, t.ServiceCard_Id })
                .ForeignKey("dbo.ServiceUsers", t => t.ServiceUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCards", t => t.ServiceCard_Id, cascadeDelete: true)
                .Index(t => t.ServiceUser_Id)
                .Index(t => t.ServiceCard_Id);
            
            CreateTable(
                "dbo.ServiceConcessionSystemCards",
                c => new
                    {
                        ServiceConcession_Id = c.Int(nullable: false),
                        SystemCard_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServiceConcession_Id, t.SystemCard_Id })
                .ForeignKey("dbo.ServiceConcessions", t => t.ServiceConcession_Id, cascadeDelete: true)
                .ForeignKey("dbo.SystemCards", t => t.SystemCard_Id, cascadeDelete: true)
                .Index(t => t.ServiceConcession_Id)
                .Index(t => t.SystemCard_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceConcessionSystemCards", "SystemCard_Id", "dbo.SystemCards");
            DropForeignKey("dbo.ServiceConcessionSystemCards", "ServiceConcession_Id", "dbo.ServiceConcessions");
            DropForeignKey("dbo.SystemCards", "ConcessionOwnerId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceUsers", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceCards", "ConcessionId", "dbo.ServiceConcessions");
            DropForeignKey("dbo.ServiceUserServiceCards", "ServiceCard_Id", "dbo.ServiceCards");
            DropForeignKey("dbo.ServiceUserServiceCards", "ServiceUser_Id", "dbo.ServiceUsers");
            DropForeignKey("dbo.ServiceCards", "SystemCardId", "dbo.SystemCards");
            DropIndex("dbo.ServiceConcessionSystemCards", new[] { "SystemCard_Id" });
            DropIndex("dbo.ServiceConcessionSystemCards", new[] { "ServiceConcession_Id" });
            DropIndex("dbo.ServiceUserServiceCards", new[] { "ServiceCard_Id" });
            DropIndex("dbo.ServiceUserServiceCards", new[] { "ServiceUser_Id" });
            DropIndex("dbo.ServiceUsers", new[] { "ConcessionId" });
            DropIndex("dbo.SystemCards", new[] { "ConcessionOwnerId" });
            DropIndex("dbo.ServiceCards", new[] { "SystemCardId" });
            DropIndex("dbo.ServiceCards", new[] { "ConcessionId" });
            DropTable("dbo.ServiceConcessionSystemCards");
            DropTable("dbo.ServiceUserServiceCards");
            DropTable("dbo.ServiceUsers");
            DropTable("dbo.SystemCards");
            DropTable("dbo.ServiceCards");
        }
    }
}
