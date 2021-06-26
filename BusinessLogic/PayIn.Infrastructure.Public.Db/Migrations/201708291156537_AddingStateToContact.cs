namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingStateToContact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "State", c => c.Int(nullable: false, defaultValue:1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "State");
        }
    }
}
