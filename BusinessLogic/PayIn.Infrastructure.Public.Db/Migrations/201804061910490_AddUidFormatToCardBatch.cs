namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUidFormatToCardBatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCardBatches", "UidFormat", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCardBatches", "UidFormat");
        }
    }
}
