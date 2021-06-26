using PayIn.Common;
using PayIn.Domain.Transport;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportCard
{
	public class TransportCardGetResult
	{
		public int Id { get; set; }
		public string DeviceName { get; set; }
		public int? DeviceEntry { get; set; }
		public DeviceTypeEnum DeviceType { get; set; }
		public string DeviceAddress { get; set; }
		public long Uid { get; set; }
		public XpDateTime LastUse { get; set; }
		public XpDateTime ExpiryDate { get; set; }
		public string Login { get; set; }
		public TransportCardState State { get; set; }
		public string ImageUrl { get; set; }
	}
}
