namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPresenceImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ControlPresences", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ControlPresences", "Image");
        }
    }
}
