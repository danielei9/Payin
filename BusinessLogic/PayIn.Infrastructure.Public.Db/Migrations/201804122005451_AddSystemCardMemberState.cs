namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSystemCardMemberState : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCardMembers", "State", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCardMembers", "State");
        }
    }
}
