using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
    public class ShopEventGetEntranceTypesArguments : IArgumentsBase
    {
        
        public int Id { get; set; }

        #region Constructors
        public ShopEventGetEntranceTypesArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
