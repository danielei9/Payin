using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportPrice
{
	public class TransportPriceDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportPriceDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
