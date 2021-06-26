namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_AddFieldInTransportOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOffers", "ImageUrl", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportOffers", "ImageUrl");
        }
    }
}
