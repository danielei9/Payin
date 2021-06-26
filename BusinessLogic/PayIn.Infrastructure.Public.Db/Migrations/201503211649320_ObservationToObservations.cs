namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservationToObservations : DbMigration
    {
        public override void Up()
        {
					RenameColumn("dbo.ControlItems", "Observation", "Observations");
					RenameColumn("dbo.ControlTemplates", "Observation", "Observations");
        }
        
        public override void Down()
        {
					RenameColumn("dbo.ControlTemplates", "Observations", "Observation");
					RenameColumn("dbo.ControlItems", "Observations", "Observation");
        }
    }
}
