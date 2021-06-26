using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobileExhibitorGetAllArguments : IArgumentsBase
	{
		public int EventId { get; set; }

		#region Constructors
		public MobileExhibitorGetAllArguments(int eventId)
		{
            EventId = eventId;
		}
		#endregion Constructors
	}
}
