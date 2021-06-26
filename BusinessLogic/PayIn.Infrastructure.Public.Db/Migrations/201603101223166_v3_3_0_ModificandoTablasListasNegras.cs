namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_ModificandoTablasListasNegras : DbMigration
    {
        public override void Up()
        {
						RenameColumn("dbo.BlackLists", "SerialNumber", "Uid");
						RenameColumn("dbo.BlackLists", "Reason", "Machine");
            DropColumn("dbo.BlackLists", "VBLN");
            DropColumn("dbo.BlackLists", "UDLN");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlackLists", "UDLN", c => c.Boolean(nullable: false));
            AddColumn("dbo.BlackLists", "VBLN", c => c.Boolean(nullable: false));
						RenameColumn("dbo.BlackLists", "Machine", "Reason");
						RenameColumn("dbo.BlackLists", "Uid", "SerialNumber");
        }
    }
}
