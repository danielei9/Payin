using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shop
{
    public class ShopGetMyCardsSelectorArguments : IArgumentsBase
    {
        public string Filter { get; set; }

        #region Constructors
        public ShopGetMyCardsSelectorArguments(string filter)
        {
            Filter = filter;
        }
        #endregion Constructors
    }
}
