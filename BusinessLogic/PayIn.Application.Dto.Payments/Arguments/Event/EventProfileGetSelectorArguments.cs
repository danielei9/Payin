using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventProfileGetSelectorArguments : IArgumentsBase
	{
        public int Id { get; set; }
        public string Filter { get; set; }

        #region Constructors
        public EventProfileGetSelectorArguments(string filter, int id)
		{
            Filter = filter;
            Id = id;
        }
		#endregion Constructors
	}
}
