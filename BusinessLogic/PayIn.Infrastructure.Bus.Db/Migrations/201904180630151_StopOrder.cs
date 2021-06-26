namespace PayIn.Infrastructure.Bus.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StopOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("Bus.Stops", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Bus.Stops", "Order");
        }
    }
}
