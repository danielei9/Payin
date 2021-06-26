using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceConcession
{
	public partial class ServiceConcessionGetResult
	{

		public int Id							{ get; set; }
		public string Name						{ get; set; }
		public ServiceType Type					{ get; set; }			
		public int MaxWorkers					{ get; set; }
		public ConcessionState State			{ get; set; }
		public string PhotoUrl                  { get; set; }
		public decimal PayinComission			{ get; set; }
		public decimal LiquidationAmountMin		{ get; set; }
	}
}