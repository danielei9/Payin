using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public class PromotionGetAllConcessionResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public int Acumulative { get; set; }
		public PromotionState State { get; set; }	
		public string ConcessionName { get; set; }
		public int CodeApplied { get; set; }
		public int TotalCode { get; set; }
		public decimal TotalCost { get; set; }
	}
}