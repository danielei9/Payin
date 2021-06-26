using PayIn.Common;
using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public class PromotionAssignResult
	{
		public class RechargePrice
		{
			public int Id { get; set; }
			public EigeZonaEnum? Zone { get; set; }
			public string ZoneName { get; set; }
			public decimal Price { get; set; }
			public decimal ChangePrice { get; set; }
			public RechargeType RechargeType { get; set; }
			public EigeTituloEnUsoEnum? Slot { get; set; }
		}
		public class RechargeTitle
		{
			public int Id { get; set; }
			public int Code { get; set; }
			public string Name { get; set; }
			public int PaymentConcessionId { get; set; }
			public int TransportConcession { get; set; }
			public string OwnerName { get; set; }
			public string OwnerCity { get; set; }
			public IEnumerable<RechargePrice> Prices { get; set; }
			public decimal? TuiNMax { get; set; }
			public decimal? TuiNMin { get; set; }
			public decimal? TuiNStep { get; set; }
			public decimal? Quantity { get; set; }
			public MeanTransportEnum? MeanTransport { get; set; }
			public decimal? MaxQuantity { get; set; }
		}
		public class PromotionAction
		{
			public PromoActionType Type { get; set; }
			public int Quantity { get; set; }
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime? EndDate { get; set; }
		public string Concession { get; set; }
		public string Image { get; set; }
		public IEnumerable<RechargeTitle> Titles { get; set; }
		public IEnumerable<PromotionAction> Actions { get; set; }
	}
}
