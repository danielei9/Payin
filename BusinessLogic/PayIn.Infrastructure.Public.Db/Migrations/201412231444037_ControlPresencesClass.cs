namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPresencesClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets");
            CreateTable(
                "dbo.ControlPresences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        Login = c.String(),
                        Date = c.DateTime(nullable: false),
                        Latitude = c.Long(nullable: false),
                        Longitude = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets");
            DropTable("dbo.ControlPresences");
            AddForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets", "Id");
        }
    }
}
