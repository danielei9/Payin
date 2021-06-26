namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_TransportTitleNullableOwnerCode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportTitles", "OwnerCode", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportTitles", "OwnerCode", c => c.Int(nullable: false));
        }
    }
}
