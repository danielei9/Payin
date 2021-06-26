namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiarDatePorCreatedAtEnIncidencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlIncidents", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlIncidents", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlIncidents", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.ControlIncidents", "CreatedAt");
        }
    }
}
