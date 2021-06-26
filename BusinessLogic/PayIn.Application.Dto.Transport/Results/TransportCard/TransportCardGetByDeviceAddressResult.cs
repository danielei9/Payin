using PayIn.Common;
using PayIn.Domain.Transport;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardGetByDeviceAddressResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? Entry { get; set; }
		public long Uid { get; set; }
		public string RandomId { get; set; }
	}
}
