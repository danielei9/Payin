// <auto-generated />
namespace PayIn.Infrastructure.JustMoney.Db.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddDepositReferenceIdToBankCard : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddDepositReferenceIdToBankCard));
        
        string IMigrationMetadata.Id
        {
            get { return "201909162039522_AddDepositReferenceIdToBankCard"; }
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
