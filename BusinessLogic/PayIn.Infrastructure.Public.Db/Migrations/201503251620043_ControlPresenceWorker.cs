namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPresenceWorker : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlPresences", "WorkerId", c => c.Int(nullable: false));
						Sql(
							"UPDATE dbo.ControlPresences " +
							"SET WorkerId = SW.Id " +
							"FROM " +
							"  dbo.ControlPresences CP INNER JOIN " +
							"  dbo.ServiceWorkers SW ON CP.Login = SW.Login "
						);
            CreateIndex("dbo.ControlPresences", "WorkerId");
            AddForeignKey("dbo.ControlPresences", "WorkerId", "dbo.ServiceWorkers", "Id");
            DropColumn("dbo.ControlPresences", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ControlPresences", "Login", c => c.String());
            DropForeignKey("dbo.ControlPresences", "WorkerId", "dbo.ServiceWorkers");
            DropIndex("dbo.ControlPresences", new[] { "WorkerId" });
            DropColumn("dbo.ControlPresences", "WorkerId");
        }
    }
}
