namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddIsOverWritableInTransportTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "IsOverWritable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "IsOverWritable");
        }
    }
}
