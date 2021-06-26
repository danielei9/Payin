namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4_1_4_EliminarServiceUserType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceCards", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ServiceCards", "LastName", c => c.String(nullable: false));
            DropColumn("dbo.ServiceUsers", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceUsers", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceCards", "LastName", c => c.String());
            AlterColumn("dbo.ServiceCards", "Name", c => c.String());
        }
    }
}
