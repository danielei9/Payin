namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_3_AddOperationSlot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "Slot", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "Slot");
        }
    }
}
