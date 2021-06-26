using PayIn.Common;
using PayIn.Domain.Public;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ControlPlanningGetAllResultBase : ResultBase<ControlPlanningGetAllResult>
	{
		public string WorkerName { get; set; }
	}
}
