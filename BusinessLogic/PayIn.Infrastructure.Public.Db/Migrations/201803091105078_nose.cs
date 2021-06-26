namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nose : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreyLists", "ObjectId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreyLists", "ObjectId");
        }
    }
}
