namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_ServiceWorkerHasAccectedToState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceWorkers", "State", c => c.Int(nullable: false));

			Sql("UPDATE SW " +
				"SET SW.State = 1 " +
				"FROM dbo.ServiceWorkers SW " +
				"WHERE SW.HasAccepted = 1 ");

			Sql("UPDATE SW " +
				"SET SW.State = 2 " +
				"FROM dbo.ServiceWorkers SW " +
				"WHERE SW.HasAccepted = 0 ");

			DropColumn("dbo.ServiceWorkers", "HasAccepted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceWorkers", "HasAccepted", c => c.Boolean(nullable: false));

			Sql("UPDATE SW " +
				"SET SW.HasAccepted = 1 " +
				"FROM dbo.ServiceWorkers SW " +
				"WHERE SW.State = 1 ");

			Sql("UPDATE SW " +
				"SET SW.HasAccepted = 0 " +
				"FROM dbo.ServiceWorkers SW " +
				"WHERE SW.State = 0 OR SW.State = 2 OR SW.State = 3 ");

			DropColumn("dbo.ServiceWorkers", "State");
        }
    }
}
