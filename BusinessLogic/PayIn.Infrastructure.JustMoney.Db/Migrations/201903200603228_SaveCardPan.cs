namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaveCardPan : DbMigration
    {
        public override void Up()
        {
            AddColumn("JustMoney.PrepaidCards", "Pan", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("JustMoney.PrepaidCards", "Pan");
        }
    }
}
