namespace PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityUpdateIdResult
	{
		public int Id { get; set; }
		public int TransportTitleId { get; set; }
		public int TransportCardSupportId { get; set; }
		public string TransportCardSupportName { get; set; }

	}
}
