namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_TransportLists : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlackLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerialNumber = c.Long(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Reason = c.Int(nullable: false),
                        Resolved = c.Boolean(nullable: false),
                        ResolutionDate = c.DateTime(),
                        Rejection = c.Boolean(nullable: false),
                        Concession = c.Int(nullable: false),
                        Service = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GreyLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerialNumber = c.Long(nullable: false),
                        OperationNumber = c.Int(nullable: false),
                        RegistrationDate = c.DateTime(nullable: false),
                        Action = c.Int(nullable: false),
                        AffectedCamp = c.String(),
                        NewValue = c.String(),
                        Resolved = c.Boolean(nullable: false),
                        ResolutionDate = c.DateTime(),
                        EquipmentType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WhiteLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardNumber = c.Int(nullable: false),
                        OperationNumber = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WhiteLists");
            DropTable("dbo.GreyLists");
            DropTable("dbo.BlackLists");
        }
    }
}
