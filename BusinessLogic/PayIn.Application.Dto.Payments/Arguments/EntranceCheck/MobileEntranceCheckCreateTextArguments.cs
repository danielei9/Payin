using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEntranceCheckCreateTextArguments : IArgumentsBase
	{
		public int EventId { get; set; }
		public string Code { get; set; }
		public string Observations { get; set; }
		public CheckInType Type { get; set; }
		public bool Force { get; set; }

		#region Constructors
		public MobileEntranceCheckCreateTextArguments(int eventId, string code, string observations, CheckInType type, bool force)
		{
			EventId = eventId;
			Code = code;
			Observations = observations ?? "";
			Type = type;
			Force = force;
		}
		#endregion Constructors
	}
}
