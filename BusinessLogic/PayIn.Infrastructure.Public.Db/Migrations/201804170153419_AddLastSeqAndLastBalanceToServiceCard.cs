namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastSeqAndLastBalanceToServiceCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceCards", "LastSeq", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.ServiceCards", "LastBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceCards", "LastBalance");
            DropColumn("dbo.ServiceCards", "LastSeq");
        }
    }
}
