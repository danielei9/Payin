using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Shop
{
    public class ShopConcessionGetEventsArguments : IArgumentsBase
    {
        // public string Filter { get; set; }
        public int Id { get; set; }

        #region Constructors
        public ShopConcessionGetEventsArguments(int id)
        {
            //Filter = filter ?? "";
            Id = id;
        }
        #endregion Constructors
    }
}
