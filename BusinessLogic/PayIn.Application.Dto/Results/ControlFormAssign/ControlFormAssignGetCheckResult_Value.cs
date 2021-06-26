using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Results.ControlFormAssign
{
	public class ControlFormAssignGetCheckResult_Value
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ControlFormArgumentType Type { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
		public decimal? ValueNumeric { get; set; }
		public string ValueString { get; set; }
	}
}
