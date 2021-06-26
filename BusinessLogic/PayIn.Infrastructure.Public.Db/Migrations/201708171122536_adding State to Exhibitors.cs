namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingStatetoExhibitors : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exhibitors", "State", c => c.Int(nullable: false , defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exhibitors", "State");
        }
    }
}
