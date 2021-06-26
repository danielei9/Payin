namespace PayIn.Infrastructure.Internal.Db.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InternalOptions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "internal.Options",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        ValueType = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("internal.Options");
        }
    }
}
