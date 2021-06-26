using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupport
{
	public class TransportCardSupportGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardSupportGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
