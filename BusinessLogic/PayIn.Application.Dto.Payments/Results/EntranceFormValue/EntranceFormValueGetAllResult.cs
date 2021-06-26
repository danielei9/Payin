using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceFormValueGetAllResult
	{
		public int Id { get; set; }
		public string ValueString { get; set; }
		public decimal? ValueNumeric { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
		public string ArgumentName { get; set; }

		public IEnumerable<EntranceFormValueGetAllResult_Option> ValueOptions { get; set; }
	}
}
