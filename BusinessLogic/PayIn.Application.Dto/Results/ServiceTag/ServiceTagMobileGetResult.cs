using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public partial class ServiceTagMobileGetResult
	{
		public int               Id        { get; set; }
		public string            Reference { get; set; }
		public IEnumerable<ServiceTagMobileGetResult_Item> Items     { get; set; }
	}
}
