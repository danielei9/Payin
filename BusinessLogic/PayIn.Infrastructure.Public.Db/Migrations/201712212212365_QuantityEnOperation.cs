namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuantityEnOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportOperations", "QuantityType", c => c.String());
            AddColumn("dbo.TransportOperations", "MobilisAmpliationBit", c => c.Boolean());
            AddColumn("dbo.TransportOperations", "MobilisEnvironment", c => c.Int());
            AlterColumn("dbo.TransportOperations", "QuantityValue", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.TransportOperations", "QuantityValueOld", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportOperations", "QuantityValueOld", c => c.Int());
            AlterColumn("dbo.TransportOperations", "QuantityValue", c => c.Int());
            DropColumn("dbo.TransportOperations", "MobilisEnvironment");
            DropColumn("dbo.TransportOperations", "MobilisAmpliationBit");
            DropColumn("dbo.TransportOperations", "QuantityType");
        }
    }
}
