namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBusToVehicle : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Bus.Buses", newName: "Vehicles");
        }
        
        public override void Down()
        {
            RenameTable(name: "Bus.Vehicles", newName: "Buses");
        }
    }
}
