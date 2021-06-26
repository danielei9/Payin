namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2_1_1_ApplicationUserNullableBirthday : DbMigration
    {
        public override void Up()
        {
            AlterColumn("security.AspNetUsers", "Birthday", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("security.AspNetUsers", "Birthday", c => c.DateTime(nullable: false));
        }
    }
}
