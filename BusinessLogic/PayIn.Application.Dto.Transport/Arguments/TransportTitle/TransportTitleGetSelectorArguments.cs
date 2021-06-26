using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{ 
	public class TransportTitleGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public TransportTitleGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
