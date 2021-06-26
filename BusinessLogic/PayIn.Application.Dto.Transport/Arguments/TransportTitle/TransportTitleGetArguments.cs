using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportTitle
{
	public class TransportTitleGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportTitleGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
