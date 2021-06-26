using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ProductGetSelectorArguments : IArgumentsBase
    {
		public int Id { get; set; }
        public string Filter { get; set; }

        #region Constructors
        public ProductGetSelectorArguments(string filter, int id)
        {
            Filter = filter;
			Id = id;
        }
        #endregion Constructors
    }
}