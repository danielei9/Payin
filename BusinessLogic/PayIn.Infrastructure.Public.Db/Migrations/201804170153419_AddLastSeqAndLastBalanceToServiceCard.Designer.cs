// <auto-generated />
namespace PayIn.Infrastructure.Public.Db.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddLastSeqAndLastBalanceToServiceCard : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddLastSeqAndLastBalanceToServiceCard));
        
        string IMigrationMetadata.Id
        {
            get { return "201804170153419_AddLastSeqAndLastBalanceToServiceCard"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}