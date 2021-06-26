namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNumberGenerationTypeToSystemCard : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SystemCards", "NumberGenerationType", c => c.Int(nullable: false, defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SystemCards", "NumberGenerationType");
        }
    }
}
