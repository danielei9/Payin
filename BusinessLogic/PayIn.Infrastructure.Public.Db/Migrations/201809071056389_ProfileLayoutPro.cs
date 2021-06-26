namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileLayoutPro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "LayoutPro", c => c.String(nullable: false));
            Sql("UPDATE dbo.Profiles SET LayoutPro=Layout");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "LayoutPro");
        }
    }
}
