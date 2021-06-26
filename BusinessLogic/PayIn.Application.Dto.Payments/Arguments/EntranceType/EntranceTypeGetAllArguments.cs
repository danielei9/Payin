using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int EventId { get; set; }

		#region Constructors
		public EntranceTypeGetAllArguments(string filter, int eventId)
		{
			Filter = filter ?? "";
			EventId = eventId;
		}
		#endregion Constructors
	}
}
