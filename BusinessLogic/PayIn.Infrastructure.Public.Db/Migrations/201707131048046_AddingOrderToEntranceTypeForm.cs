namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOrderToEntranceTypeForm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypeForms", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EntranceTypeForms", "Order");
        }
    }
}
