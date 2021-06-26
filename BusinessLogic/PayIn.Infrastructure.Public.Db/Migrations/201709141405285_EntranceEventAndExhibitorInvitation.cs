namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntranceEventAndExhibitorInvitation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Entrances", "Email", c => c.String(nullable: false, defaultValue: "''"));
            AddColumn("dbo.Exhibitors", "InvitationCode", c => c.String(nullable: false, defaultValue: "''"));

			Sql("UPDATE dbo.Entrances SET Email=Login;");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exhibitors", "InvitationCode");
            DropColumn("dbo.Entrances", "Email");
        }
    }
}
