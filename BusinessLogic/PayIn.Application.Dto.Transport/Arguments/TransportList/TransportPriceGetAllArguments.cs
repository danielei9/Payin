using PayIn.Domain.Transport;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments
{
	public partial class TransportPriceGetAllArguments : IArgumentsBase
	{
		public PayIn.Domain.Transport.TransportTitle Title { get; set; }

		#region Constructors
		public TransportPriceGetAllArguments(PayIn.Domain.Transport.TransportTitle transportTitle)
		{
			Title = transportTitle;
		}
		#endregion Constructors
	}
}
