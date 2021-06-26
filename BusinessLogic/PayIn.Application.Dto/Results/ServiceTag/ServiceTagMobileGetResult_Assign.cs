using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public class ServiceTagMobileGetResult_Assign
	{
		public int Id { get; set; }
		public string FormName { get; set; }
		public string FormObservations { get; set; }
		public IEnumerable<ServiceTagMobileGetResult_Value> Values { get; set; }
	}
}
