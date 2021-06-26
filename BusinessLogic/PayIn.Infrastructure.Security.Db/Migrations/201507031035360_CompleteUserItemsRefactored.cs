namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteUserItemsRefactored : DbMigration
    {
        public override void Up()
        {
            AddColumn("security.AspNetUsers", "Birthday", c => c.DateTime(nullable: false));
            DropColumn("security.AspNetUsers", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("security.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("security.AspNetUsers", "Birthday");
        }
    }
}
