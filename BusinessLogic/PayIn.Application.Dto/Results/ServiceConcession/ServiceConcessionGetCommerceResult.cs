using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Results.ServiceConcession
{
	public class ServiceConcessionGetCommerceResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ServiceType Type { get; set; }
		public int MaxWorkers { get; set; }
		public ConcessionState State { get; set; }
	}
}
