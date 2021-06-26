namespace PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesUpdateIdResult
    {
		public int Id { get; set; }
		public int TransportTitleId { get; set; }
		public int TransportTitle2Id { get; set; }
		public string Name { get; set; }
	}
}
