namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupEntranceTypeByEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentConcessions", "GroupEntranceTypeByEvent", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentConcessions", "GroupEntranceTypeByEvent");
        }
    }
}
