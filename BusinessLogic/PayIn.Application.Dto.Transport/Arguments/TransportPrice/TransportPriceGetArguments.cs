using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportPrice
{
	public class TransportPriceGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportPriceGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}