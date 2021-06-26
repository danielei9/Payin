using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Common.Dto.Arguments;

namespace Xp.Application.Dto.Tsm.Arguments
{
    public class TsmReadArguments : GPArguments
	{
		//public string Type { get; set; } // "InitTransaction" // "ResponseApdu"
		//public string TransactionId { get; set; } // random int
		//public string Data { get; set; } // "7Eleven"
		//public string State { get; set; }

		public NextStepEnum? NextStep { get; set; }
		//public string User { get; set; } // "Pay in"
		//public string Id { get; set; } // randId
	}
}