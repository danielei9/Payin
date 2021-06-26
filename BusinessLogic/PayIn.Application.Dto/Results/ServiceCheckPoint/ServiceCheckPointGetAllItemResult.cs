using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetItemChecksResult
	{
		public int            Id           { get; set; }
		public string         Name         { get; set; }
		public string         Reference    { get; set; }
		public decimal?       Longitude	   { get; set; }
		public decimal?       Latitude     { get; set; }
		public CheckPointType Type         { get; set; }
		public string         SupplierName { get; set; }
		public int	          FormsCount { get; set; }
	}
}
