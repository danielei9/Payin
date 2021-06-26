using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ExhibitorGetSelectorArguments : IArgumentsBase
    {
		public int Id { get; set; }
        public string Filter { get; set; }

        #region Constructors
        public ExhibitorGetSelectorArguments(string filter, int id)
        {
            Filter = filter;
			Id = id;
        }
        #endregion Constructors
    }
}