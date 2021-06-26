namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitTagCheckPoint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlPresences", "TagId", "dbo.ServiceTags");
            DropForeignKey("dbo.ControlItemServiceTags", "ControlItem_Id", "dbo.ControlItems");
            DropForeignKey("dbo.ControlItemServiceTags", "ServiceTag_Id", "dbo.ServiceTags");
            DropIndex("dbo.ControlPresences", new[] { "TagId" });
            DropIndex("dbo.ControlItemServiceTags", new[] { "ControlItem_Id" });
            DropIndex("dbo.ControlItemServiceTags", new[] { "ServiceTag_Id" });
            CreateTable(
                "dbo.ServiceCheckPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Decimal(precision: 9, scale: 6),
                        Longitude = c.Decimal(precision: 9, scale: 6),
                        Name = c.String(),
                        Observations = c.String(),
                        Type = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSuppliers", t => t.SupplierId)
                .ForeignKey("dbo.ServiceTags", t => t.TagId)
                .Index(t => t.SupplierId)
                .Index(t => t.TagId);
						Sql(
							"INSERT dbo.ServiceCheckPoints (Latitude, Longitude, Name, Observations, Type, SupplierId, TagId) " +
							"SELECT ST.Latitude, ST.Longitude, ST.Name, ST.Observations, ST.TagType, ST.SupplierId , ST.Id " +
							"FROM " +
							"	dbo.ServiceTags ST " +
							"WHERE (EXISTS( " +
							"	SELECT * " +
							"	FROM dbo.ControlItemServiceTags CIST " +
							"	WHERE CIST.ServiceTag_Id = ST.Id " +
							"))"
						);
            
            CreateTable(
                "dbo.ControlItemServiceCheckPoints",
                c => new
                    {
                        ControlItem_Id = c.Int(nullable: false),
                        ServiceCheckPoint_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ControlItem_Id, t.ServiceCheckPoint_Id })
                .ForeignKey("dbo.ControlItems", t => t.ControlItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.ServiceCheckPoints", t => t.ServiceCheckPoint_Id, cascadeDelete: true)
                .Index(t => t.ControlItem_Id)
                .Index(t => t.ServiceCheckPoint_Id);
						Sql(
							"INSERT dbo.ControlItemServiceCheckPoints (ControlItem_Id, ServiceCheckPoint_Id) " +
							"SELECT ControlItem_Id, Id " +
							"FROM " +
								"dbo.ControlItemServiceTags CIST INNER JOIN " +
								"dbo.ServiceCheckPoints SCP ON CIST.ServiceTag_Id = SCP.TagId"
						);

            AddColumn("dbo.ServiceTags", "Type", c => c.Int(nullable: false));
						Sql(
							"UPDATE dbo.ServiceTags " +
							"SET Type = TagType "
						);

            AddColumn("dbo.ControlPresences", "CheckPointId", c => c.Int());
						Sql(
							"UPDATE CP " +
							"SET CheckPointId = (" +
								"SELECT MAX(Id) " +
								"FROM dbo.ServiceCheckPoints SCP " + 
								"WHERE SCP.TagId = CP.TagId " +
							") " +
							"FROM dbo.ControlPresences CP "
						);

            AlterColumn("dbo.ServiceTags", "Reference", c => c.String(nullable: false));
            CreateIndex("dbo.ControlPresences", "CheckPointId");
            AddForeignKey("dbo.ControlPresences", "CheckPointId", "dbo.ServiceCheckPoints", "Id");
            DropColumn("dbo.ServiceTags", "Latitude");
            DropColumn("dbo.ServiceTags", "Longitude");
            DropColumn("dbo.ServiceTags", "Name");
            DropColumn("dbo.ServiceTags", "Observations");
            DropColumn("dbo.ServiceTags", "TagType");
            DropColumn("dbo.ControlPresences", "TagId");
            DropTable("dbo.ControlItemServiceTags");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ControlItemServiceTags",
                c => new
                    {
                        ControlItem_Id = c.Int(nullable: false),
                        ServiceTag_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ControlItem_Id, t.ServiceTag_Id });
            
            AddColumn("dbo.ControlPresences", "TagId", c => c.Int());
            AddColumn("dbo.ServiceTags", "TagType", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceTags", "Observations", c => c.String());
            AddColumn("dbo.ServiceTags", "Name", c => c.String());
            AddColumn("dbo.ServiceTags", "Longitude", c => c.Decimal(precision: 9, scale: 6));
            AddColumn("dbo.ServiceTags", "Latitude", c => c.Decimal(precision: 9, scale: 6));
            DropForeignKey("dbo.ControlItemServiceCheckPoints", "ServiceCheckPoint_Id", "dbo.ServiceCheckPoints");
            DropForeignKey("dbo.ControlItemServiceCheckPoints", "ControlItem_Id", "dbo.ControlItems");
            DropForeignKey("dbo.ControlPresences", "CheckPointId", "dbo.ServiceCheckPoints");
            DropForeignKey("dbo.ServiceCheckPoints", "TagId", "dbo.ServiceTags");
            DropForeignKey("dbo.ServiceCheckPoints", "SupplierId", "dbo.ServiceSuppliers");
            DropIndex("dbo.ControlItemServiceCheckPoints", new[] { "ServiceCheckPoint_Id" });
            DropIndex("dbo.ControlItemServiceCheckPoints", new[] { "ControlItem_Id" });
            DropIndex("dbo.ControlPresences", new[] { "CheckPointId" });
            DropIndex("dbo.ServiceCheckPoints", new[] { "TagId" });
            DropIndex("dbo.ServiceCheckPoints", new[] { "SupplierId" });
            AlterColumn("dbo.ServiceTags", "Reference", c => c.String());
            DropColumn("dbo.ControlPresences", "CheckPointId");
            DropColumn("dbo.ServiceTags", "Type");
            DropTable("dbo.ControlItemServiceCheckPoints");
            DropTable("dbo.ServiceCheckPoints");
            CreateIndex("dbo.ControlItemServiceTags", "ServiceTag_Id");
            CreateIndex("dbo.ControlItemServiceTags", "ControlItem_Id");
            CreateIndex("dbo.ControlPresences", "TagId");
            AddForeignKey("dbo.ControlItemServiceTags", "ServiceTag_Id", "dbo.ServiceTags", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ControlItemServiceTags", "ControlItem_Id", "dbo.ControlItems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ControlPresences", "TagId", "dbo.ServiceTags", "Id");
        }
    }
}
