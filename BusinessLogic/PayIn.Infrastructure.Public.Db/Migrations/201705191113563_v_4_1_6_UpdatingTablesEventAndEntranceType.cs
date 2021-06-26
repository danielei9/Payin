namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_UpdatingTablesEventAndEntranceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EntranceTypes", "State", c => c.Int(nullable: false));
            DropColumn("dbo.EntranceTypes", "Code");
            DropColumn("dbo.Events", "MaxEntrance");
            DropColumn("dbo.Events", "ExtraPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "ExtraPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Events", "MaxEntrance", c => c.Int());
            AddColumn("dbo.EntranceTypes", "Code", c => c.String(nullable: false));
            DropColumn("dbo.EntranceTypes", "State");
        }
    }
}
