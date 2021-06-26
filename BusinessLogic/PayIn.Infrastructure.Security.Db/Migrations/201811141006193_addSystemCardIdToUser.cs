namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSystemCardIdToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("security.AspNetUsers", "systemCardId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("security.AspNetUsers", "systemCardId");
        }
    }
}
