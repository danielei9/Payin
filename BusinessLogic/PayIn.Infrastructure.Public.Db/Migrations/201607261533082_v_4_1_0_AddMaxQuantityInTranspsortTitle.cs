namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_AddMaxQuantityInTranspsortTitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportTitles", "MaxQuantity", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportTitles", "MaxQuantity");
        }
    }
}
