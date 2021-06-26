using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Arguments.Shop
{
   public partial class ShopGetAllConcessionsArguments : IArgumentsBase
    {
        public string Filter { get; set; }
       // public int Id { get; set; }

        #region Constructors
        public ShopGetAllConcessionsArguments(string filter/*,int id */)
        {
            Filter = filter ?? "";
           // Id = id;
        }
        #endregion Constructors
    }
}
