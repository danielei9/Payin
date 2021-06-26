namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EventOperationsRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceOperations", "Event_Id", c => c.Int());
            CreateIndex("dbo.ServiceOperations", "Event_Id");
            AddForeignKey("dbo.ServiceOperations", "Event_Id", "dbo.Events", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceOperations", "Event_Id", "dbo.Events");
            DropIndex("dbo.ServiceOperations", new[] { "Event_Id" });
            DropColumn("dbo.ServiceOperations", "Event_Id");
        }
    }
}
