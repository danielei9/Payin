namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFromNodeToStop : DbMigration
    {
        public override void Up()
        {
            RenameColumn("Bus.Stops", "IsLastNodesGo", "IsLastStopsGo");
			RenameColumn("Bus.Stops", "IsLastNodesBack", "IsLastStopsBack");
        }
        
        public override void Down()
        {
			RenameColumn("Bus.Stops", "IsLastStopsBack", "IsLastNodesBack");
			RenameColumn("Bus.Stops", "IsLastStopsGo", "IsLastNodesGo");
        }
    }
}
