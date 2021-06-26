using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ServiceSupplier
{
	public class ServiceSupplierGetAllResult
    {
		public int    Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<Concession> Concessions { get; set; }		

		public class Concession 
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public ServiceType Type { get; set; }
			public string TypeName { get; set; }
			public ConcessionState State { get; set; }
			public string StateName { get; set; }
			public int? PaymentConcessionId { get; set; }
			public int MaxWorkers { get; set; }			
			public int WorkersCount { get; set; }			
			public XpDate CreateConcessionDate { get; set; }
			public decimal PayinCommission { get; set; }
			public string FormUrl { get; set; }			
		}
				
    }
}
