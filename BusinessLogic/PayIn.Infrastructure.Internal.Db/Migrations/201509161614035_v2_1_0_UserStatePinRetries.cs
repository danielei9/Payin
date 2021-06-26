namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_0_UserStatePinRetries : DbMigration
    {
        public override void Up()
        {
            AddColumn("internal.Users", "PinRetries", c => c.Int(nullable: true));
			Sql(
				"UPDATE internal.Users " +
				"SET PinRetries = 0 "				
				);
			AlterColumn("internal.Users", "PinRetries", c => c.Int(nullable: false));
            AddColumn("internal.Users", "State", c => c.Int(nullable: true));
			Sql(
				"UPDATE internal.Users " +
				"SET State = 1 "
				);
			AlterColumn("internal.Users", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("internal.Users", "State");
            DropColumn("internal.Users", "PinRetries");
        }
    }
}
