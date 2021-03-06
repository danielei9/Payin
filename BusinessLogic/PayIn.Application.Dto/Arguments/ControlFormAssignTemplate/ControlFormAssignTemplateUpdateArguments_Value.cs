using System;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignTemplate
{
	public class ControlFormAssignTemplateUpdateArguments_Value
	{
		public int Id { get; set; }
		public string ValueString { get; set; }
		public decimal? ValueNumeric { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
	}
}
