namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_ChangeNameTransportConcession : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TransportCommerces", newName: "TransportConcessions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.TransportConcessions", newName: "TransportCommerces");
        }
    }
}
