using PayIn.Common;
using System;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public partial class PromotionGetCodeResultBase : ResultBase<PromotionGetCodeResult>
	{
		public int TotalApplied { get; set; }
		public int Total { get; set; }
	}
}
