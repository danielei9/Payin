namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddDateTimeEventErrorInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "DateTimeEventError", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "DateTimeEventError");
        }
    }
}
