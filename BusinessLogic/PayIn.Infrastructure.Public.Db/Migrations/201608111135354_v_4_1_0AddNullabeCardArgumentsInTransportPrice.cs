namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0AddNullabeCardArgumentsInTransportPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "OperatorContext", c => c.Int());
            AddColumn("dbo.TransportPrices", "ValidityBit", c => c.Int());
            AddColumn("dbo.TransportPrices", "MaxExternalChanges", c => c.Int());
            AddColumn("dbo.TransportPrices", "MaxPeolpeChanges", c => c.Int());
            AddColumn("dbo.TransportPrices", "MaxTimeChanges", c => c.Int());
            AddColumn("dbo.TransportPrices", "ActiveTitle", c => c.Int());
            AddColumn("dbo.TransportPrices", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportPrices", "Priority");
            DropColumn("dbo.TransportPrices", "ActiveTitle");
            DropColumn("dbo.TransportPrices", "MaxTimeChanges");
            DropColumn("dbo.TransportPrices", "MaxPeolpeChanges");
            DropColumn("dbo.TransportPrices", "MaxExternalChanges");
            DropColumn("dbo.TransportPrices", "ValidityBit");
            DropColumn("dbo.TransportPrices", "OperatorContext");
        }
    }
}
