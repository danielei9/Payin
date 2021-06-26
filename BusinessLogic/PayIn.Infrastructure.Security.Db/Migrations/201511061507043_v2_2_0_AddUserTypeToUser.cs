namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_2_0_AddUserTypeToUser : DbMigration
    {
        public override void Up()
        {
			AddColumn("security.AspNetUsers", "isBussiness", c => c.Int(nullable: false, defaultValue: 1));
        }
        
        public override void Down()
        {
            DropColumn("security.AspNetUsers", "isBussiness");
        }
    }
}
