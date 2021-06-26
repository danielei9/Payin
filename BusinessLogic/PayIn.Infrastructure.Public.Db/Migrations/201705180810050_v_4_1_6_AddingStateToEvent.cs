namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_AddingStateToEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "State");
        }
    }
}
