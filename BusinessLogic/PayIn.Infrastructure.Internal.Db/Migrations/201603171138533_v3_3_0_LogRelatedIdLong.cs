namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3_3_0_LogRelatedIdLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("internal.Logs", "RelatedId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("internal.Logs", "RelatedId", c => c.Int(nullable: false));
        }
    }
}
