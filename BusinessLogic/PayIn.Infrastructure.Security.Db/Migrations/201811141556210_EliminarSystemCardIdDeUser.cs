namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EliminarSystemCardIdDeUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("security.AspNetUsers", "systemCardId");
        }
        
        public override void Down()
        {
            AddColumn("security.AspNetUsers", "systemCardId", c => c.Int());
        }
    }
}
