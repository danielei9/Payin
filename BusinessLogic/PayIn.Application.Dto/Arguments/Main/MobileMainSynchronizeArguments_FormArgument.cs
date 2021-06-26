using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_FormArgument
	{
		public int Id { get; set; }
		public decimal? ValueNumeric { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
		public string ValueString { get; set; }

		public IEnumerable<MobileMainSynchronizeArguments_Option> Options { get; set; }
	}
}
