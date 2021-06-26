namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWaitingTimeInStop : DbMigration
    {
        public override void Up()
        {
			AddColumn("Bus.Stops", "MasterCode", c => c.String(nullable: false));
			Sql("UPDATE Bus.Stops SET MasterCode = Code");
            AddColumn("Bus.Stops", "WaitingTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("Bus.Stops", "WaitingTime");
            DropColumn("Bus.Stops", "MasterCode");
        }
    }
}
