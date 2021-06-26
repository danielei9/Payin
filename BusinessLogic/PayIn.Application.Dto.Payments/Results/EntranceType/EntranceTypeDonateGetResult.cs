using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class EntranceTypeDonateGetResult
	{
		public int CardId { get; set; }
		//public long Uid { get; set; }
		//public string UidText { get; set; }
		public int? PurseId { get; set; }
	}
}
