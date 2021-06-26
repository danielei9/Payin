using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
    public class ShopGetEntranceArguments : IArgumentsBase
    {
        
        public int Id { get; set; }

        #region Constructors
        public ShopGetEntranceArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
