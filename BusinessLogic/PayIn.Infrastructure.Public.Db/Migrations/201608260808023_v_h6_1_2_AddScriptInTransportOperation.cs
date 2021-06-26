namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_h6_1_2_AddScriptInTransportOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "Script", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOperations", "Script");
        }
    }
}
