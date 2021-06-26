using PayIn.Common;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public class ServiceUserGetAllByDashBoardResultBase : ResultBase<ServiceUserGetAllByDashBoardResult>
	{
		public int LastDay      { get; set; }
		public int LastWeek     { get; set; }
		public int LastMonth    { get; set; }
		public int LastYear     { get; set; }
		public int PercentDay   { get; set; }
		public int PercentWeek  { get; set; }
		public int PercentMonth { get; set; }
		public int PercentYear  { get; set; }
	}
}
