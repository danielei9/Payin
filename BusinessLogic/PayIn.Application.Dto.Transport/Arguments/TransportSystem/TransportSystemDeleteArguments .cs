using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportSystem
{
	public class TransportSystemDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportSystemDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
