using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class EntranceFormValueGetAllResultBase : ResultBase<EntranceFormValueGetAllResult>
	{
		public string UserName { get; set; }
		public string EventName { get; set; }
	}
}
