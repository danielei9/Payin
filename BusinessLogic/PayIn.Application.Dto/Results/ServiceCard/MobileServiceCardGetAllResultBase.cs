using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public class MobileServiceCardGetAllResultBase : ResultBase<MobileServiceCardGetAllResult>
	{
		public int OwnerCount { get; set; }
		public int LinkedCount { get; set; }
	}
}
