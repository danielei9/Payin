namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminateLastSeqLastBalanceInServiceCard : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ServiceCards", "LastSeq");
            DropColumn("dbo.ServiceCards", "LastBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceCards", "LastBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ServiceCards", "LastSeq", c => c.Int(nullable: false));
        }
    }
}
