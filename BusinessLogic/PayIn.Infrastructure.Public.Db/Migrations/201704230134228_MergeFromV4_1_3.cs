namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MergeFromV4_1_3 : DbMigration
    {
        public override void Up()
        {
			// Esto se utiliza para hacer merge desde la v4.1.3 y por tanto ya está la migración ejecutado
			// NO DESCOMENTAR
            //AddColumn("dbo.TransportOperations", "Slot", c => c.Int());
        }
        
        public override void Down()
		{
			// Esto se utiliza para hacer merge desde la v4.1.3 y por tanto ya está la migración ejecutado
			// NO DESCOMENTAR
			//DropColumn("dbo.TransportOperations", "Slot");
        }
    }
}
