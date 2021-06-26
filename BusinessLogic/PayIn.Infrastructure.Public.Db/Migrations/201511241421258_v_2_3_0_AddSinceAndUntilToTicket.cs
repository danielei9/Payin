namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_2_3_0_AddSinceAndUntilToTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Since", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "Until", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "Until");
            DropColumn("dbo.Tickets", "Since");
        }
    }
}
