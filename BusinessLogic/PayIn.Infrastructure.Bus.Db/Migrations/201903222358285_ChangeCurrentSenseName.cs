namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCurrentSenseName : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.Vehicles", "CurrentSense", c => c.Int(nullable: false));
            DropColumn("Bus.Vehicles", "Sense");
        }
        
        public override void Down()
        {
            AddColumn("Bus.Vehicles", "Sense", c => c.Int(nullable: false));
            DropColumn("Bus.Vehicles", "CurrentSense");
        }
    }
}
