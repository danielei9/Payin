namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnyadirMovil : DbMigration
    {
        public override void Up()
        {
            AddColumn("security.AspNetUsers", "Mobile", c => c.String(nullable: false, defaultValue: "123456789"));
        }
        
        public override void Down()
        {
            DropColumn("security.AspNetUsers", "Mobile");
        }
    }
}
