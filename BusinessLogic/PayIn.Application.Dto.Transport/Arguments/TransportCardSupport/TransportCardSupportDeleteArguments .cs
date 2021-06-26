using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupport
{
	public class TransportCardSupportDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardSupportDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
