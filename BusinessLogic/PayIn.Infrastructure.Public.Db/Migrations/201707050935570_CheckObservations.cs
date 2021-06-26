namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckObservations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Checks", "Errors", c => c.String(nullable: false, defaultValue: ""));
			RenameColumn("dbo.Checks", "Observation", "Observations");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Checks", "Errors");
			RenameColumn("dbo.Checks", "Observations", "Observation");
		}
    }
}
