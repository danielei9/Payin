using System.Collections.Generic;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTypeGetSellableResult
	{
		public int		Id				{ get; set; }
		public string	Name			{ get; set; }
		public decimal	Price			{ get; set; }
		public decimal	ExtraPrice		{ get; set; }
		public decimal	TotalAmount		{ get; set; }
		public decimal	TotalExtraPrice	{ get; set; }
		public string	EventName		{ get; set; }
		public bool		Selected		{ get; set; }
	}
}
