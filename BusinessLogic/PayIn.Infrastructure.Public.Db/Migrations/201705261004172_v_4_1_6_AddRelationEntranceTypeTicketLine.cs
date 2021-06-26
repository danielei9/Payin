namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_AddRelationEntranceTypeTicketLine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketLines", "EntranceTypeId", c => c.Int(nullable: true));
            CreateIndex("dbo.TicketLines", "EntranceTypeId");
            AddForeignKey("dbo.TicketLines", "EntranceTypeId", "dbo.EntranceTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketLines", "EntranceTypeId", "dbo.EntranceTypes");
            DropIndex("dbo.TicketLines", new[] { "EntranceTypeId" });
            DropColumn("dbo.TicketLines", "EntranceTypeId");
        }
    }
}
