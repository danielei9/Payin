using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ControlPlanningGetAllResult
	{


		public int               Id            { get; set; }
		public XpDuration        SumChecks     { get; set; }
		public XpDuration        CheckDuration { get; set; }
		public XpDate            Date          { get; set; }
		public string            ItemName      { get; set; }
		public IEnumerable<ControlPlanningGetAllResult_Item> Items         { get; set; }

		#region Constructors
		public ControlPlanningGetAllResult()
		{
			Items = new List<ControlPlanningGetAllResult_Item>();
		}
		#endregion Constructors
	}
}
