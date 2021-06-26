using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
   public partial class ShopEventGetAllArguments : IArgumentsBase
    {
       // public string Filter { get; set; }
        public int Id { get; set; }

        #region Constructors
        public ShopEventGetAllArguments(int id )
        {
            //Filter = filter ?? "";
            Id = id;
        }
        #endregion Constructors
    }
}
