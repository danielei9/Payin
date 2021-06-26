using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityCreateGetNameArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportCardSupportTitleCompatibilityCreateGetNameArguments(int id)
		{
			Id = Id;
		}
		#endregion Constructors
	}
}
