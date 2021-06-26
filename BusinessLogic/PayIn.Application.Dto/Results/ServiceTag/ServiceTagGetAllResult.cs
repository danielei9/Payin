using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public partial class ServiceTagGetAllResult
	{
		public int            Id           { get; set; }
		public string         Reference	   { get; set; }
		public CheckPointType Type         { get; set; }
		public string         TypeLabel    { get; set; }
		public int            SupplierId   { get; set; }
		public string         SupplierName { get; set; }
	}
}
