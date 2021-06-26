namespace PayIn.Infrastructure.Security.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteUserInfoPhotoUrlType : DbMigration
    {
        public override void Up()
        {
            AddColumn("security.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("security.AspNetUsers", "PhotoUrl", c => c.String(nullable: false));
            AddColumn("security.AspNetUsers", "Sex", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("security.AspNetUsers", "Sex");
            DropColumn("security.AspNetUsers", "PhotoUrl");
            DropColumn("security.AspNetUsers", "BirthDate");
        }
    }
}
