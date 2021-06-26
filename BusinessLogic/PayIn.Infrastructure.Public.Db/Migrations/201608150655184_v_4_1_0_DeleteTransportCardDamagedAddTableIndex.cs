namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_0_DeleteTransportCardDamagedAddTableIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportPrices", "TableIndex", c => c.Int());
            DropTable("dbo.TransportCardDamageds");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransportCardDamageds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Int(nullable: false),
                        AdditionDate = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.TransportPrices", "TableIndex");
        }
    }
}
