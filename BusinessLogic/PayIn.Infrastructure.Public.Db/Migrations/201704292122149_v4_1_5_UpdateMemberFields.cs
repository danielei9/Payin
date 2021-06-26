namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_5_UpdateMemberFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceUsers", "Code", c => c.String(nullable: false));
            AddColumn("dbo.ServiceUsers", "Observations", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceUsers", "Observations");
            DropColumn("dbo.ServiceUsers", "Code");
        }
    }
}
