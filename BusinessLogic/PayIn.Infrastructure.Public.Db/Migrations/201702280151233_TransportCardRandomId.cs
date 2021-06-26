namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportCardRandomId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportCards", "RandomId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportCards", "RandomId", c => c.Int());
        }
    }
}
