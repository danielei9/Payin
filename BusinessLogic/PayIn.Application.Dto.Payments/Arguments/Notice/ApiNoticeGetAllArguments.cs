using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiNoticeGetAllArguments : IArgumentsBase
	{
		public int EventId { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public ApiNoticeGetAllArguments(string filter, int eventId)
		{
			EventId = eventId;
			Filter = filter ?? "";
        }
		#endregion Constructors
	}
}
