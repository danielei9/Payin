using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportPrice
{
	public class TransportPriceGetAllArguments : IArgumentsBase
	{		
		public int TitleId { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public TransportPriceGetAllArguments(int titleId, string filter)
		{
			TitleId = titleId;
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
