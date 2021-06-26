using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Payments.Results.PaymentUser
{
	public class PaymentUserGetAllResult
	{
		public int Id { set; get; }
		public string Name { set; get; }
		public string Login { set; get; }
		public PaymentUserState State { set; get; }
	}
}
