using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Payments.Results.PaymentUser
{
	public class PaymentUserMobileGetAllConcessionResult
	{
		public int Id { get; set; }
		public int ConcessionId { get; set; }
		public string SupplierName { get; set; }
		public string ConcessionName { get; set; }
		public PaymentUserState State { set; get; }
	}
}
