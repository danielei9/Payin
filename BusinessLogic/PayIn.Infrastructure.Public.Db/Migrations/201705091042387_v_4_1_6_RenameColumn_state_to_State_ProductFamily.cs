namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v_4_1_6_RenameColumn_state_to_State_ProductFamily : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ProductFamilies", "state", "State");
        }
        
        public override void Down()
        {
           RenameColumn("dbo.ProductFamilies", "State", "state");
        }
    }
}
