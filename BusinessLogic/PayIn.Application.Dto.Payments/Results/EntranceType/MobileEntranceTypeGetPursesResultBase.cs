using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileEntranceTypeGetPursesResultBase : ResultBase<MobileEntranceTypeGetPursesResult>
	{
		public int PurseId { get; set; }
		public string Name { get; set; }

	}
}
