using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{
	public partial class PromotionGetTitleSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public PromotionGetTitleSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}

