namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_1_AddNullableFieldSubTypeFromTransportCardSupport : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportCardSupports", "SubType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportCardSupports", "SubType", c => c.Int(nullable: false));
        }
    }
}
