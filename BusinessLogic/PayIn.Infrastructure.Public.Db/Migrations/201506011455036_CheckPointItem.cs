namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckPointItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ControlItemServiceCheckPoints", "ControlItem_Id", "dbo.ControlItems");
            DropForeignKey("dbo.ControlItemServiceCheckPoints", "ServiceCheckPoint_Id", "dbo.ServiceCheckPoints");
            DropIndex("dbo.ControlItemServiceCheckPoints", new[] { "ControlItem_Id" });
            DropIndex("dbo.ControlItemServiceCheckPoints", new[] { "ServiceCheckPoint_Id" });
            AddColumn("dbo.ServiceCheckPoints", "ItemId", c => c.Int(nullable: false));
						Sql(
							"UPDATE SCP " +
							"SET ItemId = CISCP.ControlItem_Id " +
							"FROM " + 
								"dbo.ServiceCheckPoints SCP INNER JOIN " +
								"dbo.ControlItemServiceCheckPoints CISCP ON CISCP.ServiceCheckPoint_Id = SCP.Id "
						);
            AddColumn("dbo.ControlTrackItems", "Speed", c => c.Single(nullable: false));
            CreateIndex("dbo.ServiceCheckPoints", "ItemId");
            AddForeignKey("dbo.ServiceCheckPoints", "ItemId", "dbo.ControlItems", "Id");
            DropTable("dbo.ControlItemServiceCheckPoints");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ControlItemServiceCheckPoints",
                c => new
                    {
                        ControlItem_Id = c.Int(nullable: false),
                        ServiceCheckPoint_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ControlItem_Id, t.ServiceCheckPoint_Id });
            
            DropForeignKey("dbo.ServiceCheckPoints", "ItemId", "dbo.ControlItems");
            DropIndex("dbo.ServiceCheckPoints", new[] { "ItemId" });
            DropColumn("dbo.ControlTrackItems", "Speed");
            DropColumn("dbo.ServiceCheckPoints", "ItemId");
            CreateIndex("dbo.ControlItemServiceCheckPoints", "ServiceCheckPoint_Id");
            CreateIndex("dbo.ControlItemServiceCheckPoints", "ControlItem_Id");
            AddForeignKey("dbo.ControlItemServiceCheckPoints", "ServiceCheckPoint_Id", "dbo.ServiceCheckPoints", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ControlItemServiceCheckPoints", "ControlItem_Id", "dbo.ControlItems", "Id", cascadeDelete: true);
        }
    }
}
