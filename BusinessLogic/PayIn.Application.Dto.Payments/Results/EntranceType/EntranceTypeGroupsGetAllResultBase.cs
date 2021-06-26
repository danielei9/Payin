using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTypeGroupsGetAllResultBase : ResultBase<EntranceTypeGroupsGetAllResult>
	{
		public string EntranceTypeName { get; set; }
		public string EventName { get; set; }
	}
}
