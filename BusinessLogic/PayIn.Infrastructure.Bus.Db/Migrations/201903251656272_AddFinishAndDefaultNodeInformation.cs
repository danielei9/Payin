namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFinishAndDefaultNodeInformation : DbMigration
    {
        public override void Up()
		{
			AddColumn("Bus.Stops", "IsDefaultStop", c => c.Boolean(nullable: true));
			AddColumn("Bus.Stops", "IsLastNodesGo", c => c.Boolean(nullable: true));
			AddColumn("Bus.Stops", "IsLastNodesBack", c => c.Boolean(nullable: true));
			AddColumn("Bus.RequestStops", "State", c => c.Int(nullable: true));

			Sql(
				"UPDATE Bus.Stops " +
				"SET IsDefaultStop=0, IsLastNodesGo=0, IsLastNodesBack=0 "
			);
			Sql(
				"UPDATE Bus.RequestStops " +
				"SET State=1 "
			);

			AlterColumn("Bus.Stops", "IsDefaultStop", c => c.Boolean(nullable: false));
            AlterColumn("Bus.Stops", "IsLastNodesGo", c => c.Boolean(nullable: false));
            AlterColumn("Bus.Stops", "IsLastNodesBack", c => c.Boolean(nullable: false));
            AlterColumn("Bus.RequestStops", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Bus.RequestStops", "State");
            DropColumn("Bus.Stops", "IsLastNodesBack");
            DropColumn("Bus.Stops", "IsLastNodesGo");
            DropColumn("Bus.Stops", "IsDefaultStop");
        }
    }
}
