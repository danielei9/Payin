using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
