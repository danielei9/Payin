using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ApiCampaignGetAddOwnersResultBase : ResultBase<ApiCampaignGetAddOwnersResult>
	{
		public class PaymentConcession
		{
			public int Id { set; get; }
			public string Name { set; get; }
			public string Login { set; get; }
		}

		public List<PaymentConcession> PaymentConcessions { get; set; }
	}
}