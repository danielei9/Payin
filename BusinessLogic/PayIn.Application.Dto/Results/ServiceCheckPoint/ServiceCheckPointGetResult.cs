using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetResult
	{
		public int            Id           { get; set; }
		public string         TagReference { get; set; }
		public int?           TagId        { get; set; }
		public CheckPointType Type         { get; set; }
		public string         Name         { get; set; }
		public string         Observations { get; set; }
		public decimal?       Longitude    { get; set; }
		public decimal?       Latitude	   { get; set; }
	}
}
