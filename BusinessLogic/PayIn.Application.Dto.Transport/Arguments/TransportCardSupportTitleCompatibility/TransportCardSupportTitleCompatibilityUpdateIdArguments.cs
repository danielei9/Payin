using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityUpdateIdArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityUpdateIdArguments(int id)
		{
			Id = Id;
		}
		#endregion Constructors
	}
}
