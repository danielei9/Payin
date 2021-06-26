namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_2_AddErrorInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "Error", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "Error");
        }
    }
}
