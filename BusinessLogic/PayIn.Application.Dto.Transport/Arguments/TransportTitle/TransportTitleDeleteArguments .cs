using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{
	public class TransportTitleDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportTitleDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
