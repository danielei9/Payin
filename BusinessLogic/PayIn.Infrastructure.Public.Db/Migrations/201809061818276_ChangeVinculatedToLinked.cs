namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeVinculatedToLinked : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ServiceUserVinculations", newName: "ServiceUserLinks");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ServiceUserLinks", newName: "ServiceUserVinculations");
        }
    }
}
