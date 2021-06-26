using Xp.Application.Dto;

namespace PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesGetByTitleResultBase : ResultBase<TransportSimultaneousTitleCompatibilitiesGetByTitleResult>
	{
		public string TitleName { get; set; }
	}
}
