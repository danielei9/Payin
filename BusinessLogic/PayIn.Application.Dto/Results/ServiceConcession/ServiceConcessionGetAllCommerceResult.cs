using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ServiceConcession
{
	public class ServiceConcessionGetAllCommerceResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ServiceType Type { get; set; }
		public string TypeName { get; set; }
		public int SupplierId { get; set; }
		public ConcessionState State { get; set; }
		public string SupplierName { get; set; }
		public decimal PayinCommision { get; set; }
		public XpDate CreateConcessionDate { get; set; }
	}
}
