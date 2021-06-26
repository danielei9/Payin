using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
