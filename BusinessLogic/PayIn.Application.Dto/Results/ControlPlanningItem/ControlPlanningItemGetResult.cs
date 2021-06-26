using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPlanningItem
{
	public partial class ControlPlanningItemGetResult
	{
		public int        Id              { get; set; }
		public int        SinceId         { get; set; }
		public XpDateTime Since           { get; set; }
		public int        SinceFormsCount { get; set; }
		public int        UntilId         { get; set; }
		public XpDateTime Until           { get; set; }
		public int        UntilFormsCount { get; set; }
	}
}
