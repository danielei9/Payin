using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportValidation
{
	public class TransportValidationGetAllResultBase : ResultBase<TransportValidationGetAllResult>
	{				
		public class ValidationsYesterday
		{
			public int Hour { get; set; }
			public decimal Value { get; set; }
		}

		public class ValidationsLastWeek
		{
			public int? DayOfWeek { get; set; }
			public DateTime Date { get; set; }
			public decimal? Value { get; set; }
		}
		public class ValidationsLastMonth
		{
			public XpDate Date { get; set; }
			public decimal? Value { get; set; }
		}

		public IEnumerable<ValidationsYesterday> ValidationYesterday { get; set; }
		public IEnumerable<ValidationsLastWeek> ValidationLastWeek { get; set; }
		public IEnumerable<ValidationsLastMonth> ValidationLastMonth { get; set; }
	}
}
