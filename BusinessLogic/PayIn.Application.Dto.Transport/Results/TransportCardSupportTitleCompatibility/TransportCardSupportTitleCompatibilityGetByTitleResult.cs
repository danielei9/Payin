using PayIn.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportCardSupportTitleCompatibility
{
	public class TransportCardSupportTitleCompatibilityGetByTitleResult
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public int OwnerCode { get; set; }
		public string OwnerName { get; set; }
		public int Type { get; set; }
		public int? SubType { get; set; }
		public TransportCardSupportState State { get; set; }
	}
}
