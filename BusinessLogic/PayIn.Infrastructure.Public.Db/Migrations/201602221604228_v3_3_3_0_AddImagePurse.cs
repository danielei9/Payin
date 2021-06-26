namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_3_0_AddImagePurse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Purses", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Purses", "Image");
        }
    }
}
