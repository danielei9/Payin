using Xp.Application.Dto;

namespace PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityGetByTitleResultBase : ResultBase<TransportCardSupportTitleCompatibilityGetByTitleResult>
	{
		public string TitleName { get; set; }
	}
}
