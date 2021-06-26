using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusMobileRequestGetByUserResultBase : ResultBase<BusMobileRequestGetByUserResult>
	{
		public IEnumerable<BusApiStopGetByLineResult> Stops { get; set; }
	}
}
