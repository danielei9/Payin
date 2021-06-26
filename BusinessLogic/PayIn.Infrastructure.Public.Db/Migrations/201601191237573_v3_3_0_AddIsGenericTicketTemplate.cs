namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddIsGenericTicketTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TicketTemplates", "IsGeneric", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TicketTemplates", "IsGeneric");
        }
    }
}
