using PayIn.Domain.Transport.Eige.Enums;
using System;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public class PromotionGetCodeResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public DateTime? AppliedDate { get; set; }	
		public string Title { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public string ZoneName { get; set; }
		public decimal? Price { get; set; }
        public decimal? Travels { get; set; }
        public decimal? TravelsPrice { get; set; }
	   

	}
}