using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
    public class ShopGetSelectorArguments : IArgumentsBase
    {
        public string Filter { get; set; }

        #region Constructors
        public ShopGetSelectorArguments(string filter)
        {
            Filter = filter;
        }
        #endregion Constructors
    }
}
