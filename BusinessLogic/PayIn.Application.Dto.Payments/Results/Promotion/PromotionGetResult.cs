using PayIn.Common;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public class PromotionGetResult
	{
		public class TitleList
		{
			public string Title { get; set; }
			public decimal Price { get; set; }
			public string Zone { get; set; }
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Acumulative { get; set; }
		public PromotionState State { get; set; }
		public string Title { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public decimal Price { get; set; }
		public dynamic PromoConditions { get; set; }
		public int PromoActions { get; set; }
		public dynamic PromoLaunchers { get; set; }
		public string ConcessionName { get; set; }
		public bool isOwner { get; set; }
		public int TitleQuantity { get; set; }
		public IEnumerable<TitleList> TitlesList { get; set; }
	}
}