using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{ 
	public class TransportTitleGetSelectorTConcessionArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public TransportTitleGetSelectorTConcessionArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
