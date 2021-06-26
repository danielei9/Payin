using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCard
{
	public class TransportCardDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
