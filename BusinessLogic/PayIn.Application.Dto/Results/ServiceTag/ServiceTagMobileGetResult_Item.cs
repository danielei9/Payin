using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public class ServiceTagMobileGetResult_Item
	{
		public int Id { get; set; }
		public PresenceType? PresenceType { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public bool SaveTrack { get; set; }
		public bool SaveFacialRecognition { get; set; }
		public int CheckPointId { get; set; }
		public IEnumerable<ServiceTagMobileGetResult_Planning> Plannings { get; set; }
	}
}
