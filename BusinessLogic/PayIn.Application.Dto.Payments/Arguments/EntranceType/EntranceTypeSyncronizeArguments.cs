using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeSyncronizeArguments : IArgumentsBase
	{
		public int EventId { get; set; }
		public IEnumerable<EntranceTypeSyncronizeArguments_EntranceType> EntranceTypes { get; set; }
	}
}
