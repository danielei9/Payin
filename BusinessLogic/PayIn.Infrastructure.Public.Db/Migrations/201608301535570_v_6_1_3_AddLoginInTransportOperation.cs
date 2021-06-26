namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_6_1_3_AddLoginInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "Login", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "Login");
        }
    }
}
